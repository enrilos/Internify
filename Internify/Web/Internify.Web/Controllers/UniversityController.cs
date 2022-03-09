namespace Internify.Web.Controllers
{
    using Common;
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Candidate;
    using Internify.Models.InputModels.University;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.CandidateUniversity;
    using Services.Country;
    using Services.Specialization;
    using Services.University;

    using static Common.WebConstants;

    public class UniversityController : Controller
    {
        private readonly IUniversityService universityService;
        private readonly ICountryService countryService;
        private readonly ISpecializationService specializationService;
        private readonly ICandidateUniversityService candidateUniversityService;
        private readonly IMemoryCache cache;
        private readonly RoleChecker roleChecker;

        public UniversityController(
            IUniversityService universityService,
            ICountryService countryService,
            ISpecializationService specializationService,
            ICandidateUniversityService candidateUniversityService,
            IMemoryCache cache,
            RoleChecker roleChecker)
        {
            this.universityService = universityService;
            this.countryService = countryService;
            this.specializationService = specializationService;
            this.candidateUniversityService = candidateUniversityService;
            this.cache = cache;
            this.roleChecker = roleChecker;
        }

        public IActionResult All([FromQuery] UniversityListingQueryModel queryModel)
        {
            // filter by type? private vs public

            queryModel = universityService.All(
                queryModel.Name,
                queryModel.CountryId,
                queryModel.CurrentPage,
                queryModel.UniversitiesPerPage);

            queryModel.Countries = AcquireCachedCountries();

            return View(queryModel);
        }

        [Authorize]
        public IActionResult Register()
        {
            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(User.Id()))
            {
                return BadRequest();
            }

            var universityModel = new RegisterUniversityFormModel
            {
                Countries = AcquireCachedCountries()
            };

            return View(universityModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Register(RegisterUniversityFormModel university)
        {
            var userId = User.Id();

            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(userId))
            {
                return BadRequest();
            }

            if (!countryService.Exists(university.CountryId))
            {
                ModelState.AddModelError(nameof(university.CountryId), "Invalid option.");
            }

            if (!ModelState.IsValid)
            {
                university.Countries = AcquireCachedCountries();

                return View(university);
            }

            var universityId = universityService.Add(
                userId,
                university.Name,
                university.ImageUrl,
                university.WebsiteUrl,
                university.Founded,
                university.Type,
                university.Description,
                university.CountryId);

            TempData[GlobalMessageKey] = "Thank you for registering your university!";

            return RedirectToAction(nameof(Details), new { id = universityId });
        }

        public IActionResult Details(string id)
        {
            var university = universityService.GetDetailsModel(id);

            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!IsTheSameUniversity(id))
            {
                return Unauthorized();
            }

            var university = universityService.GetEditModel(id);

            if (university == null)
            {
                return NotFound();
            }

            university.Countries = AcquireCachedCountries();

            return View(university);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditUniversityFormModel university)
        {
            if (!IsTheSameUniversity(university.Id))
            {
                return Unauthorized();
            }

            if (!countryService.Exists(university.CountryId))
            {
                ModelState.AddModelError(nameof(university.CountryId), "Invalid option.");
            }

            if (!ModelState.IsValid)
            {
                university.Countries = AcquireCachedCountries();

                return View(university);
            }

            var editResult = universityService.Edit(
                university.Id,
                university.Name,
                university.ImageUrl,
                university.WebsiteUrl,
                university.Founded,
                university.Type,
                university.Description,
                university.CountryId);

            if (!editResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully edited university.";

            return RedirectToAction(nameof(Details), new { id = university.Id });
        }

        public IActionResult Alumni([FromQuery] CandidateListingQueryModel queryModel)
        {
            if (!universityService.Exists(queryModel.UniversityId))
            {
                return NotFound();
            }

            queryModel = universityService.Alumni(
                queryModel.UniversityId,
                queryModel.FirstName,
                queryModel.LastName,
                queryModel.SpecializationId,
                queryModel.CountryId,
                queryModel.IsAvailable,
                queryModel.CurrentPage,
                queryModel.CandidatesPerPage);

            queryModel.Countries = AcquireCachedCountries();
            queryModel.Specializations = AcquireCachedSpecializations();

            return View(queryModel);
        }

        [Authorize]
        public IActionResult AddToAlumni(string universityId, string candidateId)
        {
            if (!IsTheSameUniversity(universityId))
            {
                return Unauthorized();
            }

            if (candidateUniversityService.IsCandidateInUniversityAlumni(universityId, candidateId))
            {
                return BadRequest();
            }

            var result = candidateUniversityService.AddCandidateToAlumni(universityId, candidateId);

            if (!result)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Candidate was successfully added to your alumni!";

            return RedirectToAction(nameof(Details), new { id = universityId });
        }

        [Authorize]
        public IActionResult RemoveFromAlumni(string universityId, string candidateId)
        {
            if (!IsTheSameUniversity(universityId))
            {
                return Unauthorized();
            }

            if (!candidateUniversityService.IsCandidateInUniversityAlumni(universityId, candidateId))
            {
                return NotFound();
            }

            var result = candidateUniversityService.RemoveCandidateFromAlumni(universityId, candidateId);

            if (!result)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Candidate was successfully removed from your alumni!";

            return RedirectToAction(nameof(Details), new { id = universityId });
        }

        private bool IsTheSameUniversity(string id)
            => universityService
            .GetIdByUserId(User?.Id()) == id;

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