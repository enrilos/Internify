namespace Internify.Web.Controllers
{
    using Common;
    using Infrastructure.Extensions;
    using Internify.Data.Models;
    using Internify.Models.InputModels.Candidate;
    using Internify.Models.InputModels.University;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Data.Country;
    using Services.Data.Specialization;
    using Services.Data.University;

    using static Common.WebConstants;

    public class UniversityController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUniversityService universityService;
        private readonly ICountryService countryService;
        private readonly ISpecializationService specializationService;
        private readonly IMemoryCache cache;
        private readonly RoleChecker roleChecker;

        public UniversityController(
            SignInManager<ApplicationUser> signInManager,
            IUniversityService universityService,
            ICountryService countryService,
            ISpecializationService specializationService,
            IMemoryCache cache,
            RoleChecker roleChecker)
        {
            this.signInManager = signInManager;
            this.universityService = universityService;
            this.countryService = countryService;
            this.specializationService = specializationService;
            this.cache = cache;
            this.roleChecker = roleChecker;
        }

        public IActionResult All([FromQuery] UniversityListingQueryModel queryModel)
        {
            queryModel = universityService.All(
                queryModel.Name,
                queryModel.Type,
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

            var result = universityService.Add(
                userId,
                university.Name,
                university.ImageUrl,
                university.WebsiteUrl,
                university.Founded,
                university.Type,
                university.Description,
                university.CountryId);

            if (result == null)
            {
                return BadRequest();
            }

            Task.Run(async () =>
            {
                await signInManager.SignOutAsync();
            })
            .GetAwaiter()
            .GetResult();

            TempData[GlobalMessageKey] = "Thank you for registering your university!";

            return RedirectToAction("Index", "Home");
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