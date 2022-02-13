namespace Internify.Web.Controllers
{
    using Common;
    using Data;
    using Data.Models;
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

        // Add filters (by country, specialization.., isAvailable) with query model class.
        // Moreover, add firstName and lastName search with .Contains()
        // https://stackoverflow.com/questions/32639759/search-bar-with-filter-bootstrap
        public IActionResult All([FromQuery] CandidateListingQueryModel queryModel)
        {
            var candidates = candidateService
                .All(queryModel.FullName, queryModel.IsAvailable, queryModel.SpecializationId, queryModel.CountryId);

            return View(candidates);
        }

        [Authorize]
        public IActionResult Become()
        {
            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(User.Id()))
            {
                return BadRequest();
            }

            var candidateModel = new BecomeCandidateFormModel();

            candidateModel.Specializations = AcquireCachedSpecializations();
            candidateModel.Countries = AcquireCachedCountries();

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

            var candidateData = new Candidate
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Description = candidate.Description,
                ImageUrl = candidate.ImageUrl,
                WebsiteUrl = candidate.WebsiteUrl,
                BirthDate = candidate.BirthDate,
                Gender = candidate.Gender,
                SpecializationId = candidate.SpecializationId,
                CountryId = candidate.CountryId
            };

            candidateData.UserId = userId;

            if (candidateData.ImageUrl == null)
            {
                candidateData.ImageUrl = Path.Combine(HttpContext.Request.Host.Value, "/images/avatar.png");
            }

            data.Candidates.Add(candidateData);
            data.SaveChanges();

            TempData[GlobalMessageKey] = "Thank you for becoming a candidate!";

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(string id)
        {
            var candidate = candidateService.Get(id);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        public IEnumerable<SpecializationListingViewModel> AcquireCachedSpecializations()
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

        public IEnumerable<CountryListingViewModel> AcquireCachedCountries()
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