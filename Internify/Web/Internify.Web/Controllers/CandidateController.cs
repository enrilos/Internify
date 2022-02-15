namespace Internify.Web.Controllers
{
    using Common;
    using Data;
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Candidate;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Candidate;
    using Services.Country;
    using Services.Specialization;

    using static Common.WebConstants;

    public class CandidateController : Controller
    {
        private readonly InternifyDbContext data;
        private readonly RoleChecker roleChecker;
        private readonly IMemoryCache cache;
        private readonly ICountryService countryService;
        private readonly ISpecializationService specializationService;
        private readonly ICandidateService candidateService;

        public CandidateController(
            InternifyDbContext data,
            RoleChecker roleChecker,
            IMemoryCache cache,
            ICountryService countryService,
            ISpecializationService specializationService,
            ICandidateService candidateService)
        {
            this.data = data;
            this.roleChecker = roleChecker;
            this.cache = cache;
            this.countryService = countryService;
            this.specializationService = specializationService;
            this.candidateService = candidateService;
        }

        public IActionResult All([FromQuery] CandidateListingQueryModel queryModel)
        {
            queryModel =
                candidateService
                .All(
                queryModel.FirstName,
                queryModel.LastName,
                queryModel.SpecializationId,
                queryModel.CountryId,
                queryModel.IsAvailable,
                queryModel.CurrentPage,
                queryModel.CandidatesPerPage);

            queryModel.Specializations = AcquireCachedSpecializations();
            queryModel.Countries = AcquireCachedCountries();

            return View(queryModel);
        }

        [Authorize]
        public IActionResult Become()
        {
            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(User.Id()))
            {
                return BadRequest();
            }

            var candidateModel = new BecomeCandidateFormModel
            {
                Specializations = AcquireCachedSpecializations(),
                Countries = AcquireCachedCountries()
            };

            return View(candidateModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeCandidateFormModel candidate)
        {
            var userId = User.Id();

            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(userId))
            {
                return BadRequest();
            }

            var candidateBirthDateBelowMax = candidate.BirthDate < MaxDateAllowed;
            var candidateBirthDateOverMin = candidate.BirthDate > MinDateAllowed;

            if (!candidateBirthDateBelowMax
                || !candidateBirthDateOverMin)
            {
                ModelState.AddModelError(nameof(candidate.BirthDate), $"Birth Date should be between {MinDateAllowed.ToString("MM/dd/yyyy")} and {MaxDateAllowed.ToString("MM/dd/yyyy")}.");
            }

            if (!specializationService.Exists(candidate.SpecializationId))
            {
                ModelState.AddModelError(nameof(candidate.SpecializationId), "Invalid option.");
            }

            if (!countryService.Exists(candidate.CountryId))
            {
                ModelState.AddModelError(nameof(candidate.CountryId), "Invalid option.");
            }

            if (!ModelState.IsValid)
            {
                candidate.Specializations = AcquireCachedSpecializations();
                candidate.Countries = AcquireCachedCountries();

                return View(candidate);
            }

            candidateService.Add(
                userId,
                candidate.FirstName,
                candidate.LastName,
                candidate.Description,
                candidate.ImageUrl,
                candidate.WebsiteUrl,
                candidate.BirthDate,
                candidate.Gender,
                candidate.SpecializationId,
                candidate.CountryId,
                HttpContext.Request.Host.Value);

            TempData[GlobalMessageKey] = "Thank you for becoming a candidate!";

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(string id)
        {
            var candidate = candidateService.GetDetailsModel(id);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!IsTheSameCandidate(id))
            {
                return Unauthorized();
            }

            var candidate = candidateService.GetEditModel(id);

            candidate.Specializations = AcquireCachedSpecializations();
            candidate.Countries = AcquireCachedCountries();

            return View(candidate);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditCandidateFormModel candidate)
        {
            if (!IsTheSameCandidate(candidate.Id))
            {
                return Unauthorized();
            }

            // TODO: abstract recurring code.
            var candidateBirthDateBelowMax = candidate.BirthDate < MaxDateAllowed;
            var candidateBirthDateOverMin = candidate.BirthDate > MinDateAllowed;

            if (!candidateBirthDateBelowMax
                || !candidateBirthDateOverMin)
            {
                ModelState.AddModelError(nameof(candidate.BirthDate), $"Birth Date should be between {MinDateAllowed.ToString("MM/dd/yyyy")} and {MaxDateAllowed.ToString("MM/dd/yyyy")}.");
            }

            if (!specializationService.Exists(candidate.SpecializationId))
            {
                ModelState.AddModelError(nameof(candidate.SpecializationId), "Invalid option.");
            }

            if (!countryService.Exists(candidate.CountryId))
            {
                ModelState.AddModelError(nameof(candidate.CountryId), "Invalid option.");
            }

            if (!ModelState.IsValid)
            {
                candidate.Specializations = AcquireCachedSpecializations();
                candidate.Countries = AcquireCachedCountries();

                return View(candidate);
            }

            var editResult = candidateService.Edit(
                candidate.Id,
                candidate.FirstName,
                candidate.LastName,
                candidate.Description,
                candidate.ImageUrl,
                candidate.WebsiteUrl,
                candidate.BirthDate,
                candidate.Gender,
                candidate.IsAvailable,
                candidate.SpecializationId,
                candidate.CountryId);

            if (!editResult)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        private bool IsTheSameCandidate(string id)
        {
            var currentUserId = candidateService.GetIdByUserId(User?.Id());

            if (currentUserId != id)
            {
                return false;
            }

            return true;
        }

        private IEnumerable<SpecializationListingViewModel> AcquireCachedSpecializations()
        {
            var specializations = cache.Get<IEnumerable<SpecializationListingViewModel>>(SpecializationsCacheKey);

            if (specializations == null)
            {
                specializations = specializationService.All();

                var cacheOptions = new MemoryCacheEntryOptions()
                     .SetPriority(CacheItemPriority.NeverRemove);

                cache.Set(SpecializationsCacheKey, specializations, cacheOptions);
            }

            return specializations;
        }

        private IEnumerable<CountryListingViewModel> AcquireCachedCountries()
        {
            var countries = cache.Get<IEnumerable<CountryListingViewModel>>(CountriesCacheKey);

            if (countries == null)
            {
                countries = countryService.All();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.NeverRemove);

                cache.Set(CountriesCacheKey, countries, cacheOptions);
            }

            return countries;
        }
    }
}