namespace Internify.Web.Controllers
{
    using Common;
    using Infrastructure.Extensions;
    using Internify.Data.Models;
    using Internify.Models.InputModels.Company;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Data.Company;
    using Services.Data.Country;
    using Services.Data.Specialization;

    using static Common.WebConstants;

    public class CompanyController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICompanyService companyService;
        private readonly ISpecializationService specializationService;
        private readonly ICountryService countryService;
        private readonly IMemoryCache cache;
        private readonly RoleChecker roleChecker;

        public CompanyController(
            SignInManager<ApplicationUser> signInManager,
            ICompanyService companyService,
            ISpecializationService specializationService,
            ICountryService countryService,
            IMemoryCache cache,
            RoleChecker roleChecker)
        {
            this.signInManager = signInManager;
            this.companyService = companyService;
            this.specializationService = specializationService;
            this.countryService = countryService;
            this.cache = cache;
            this.roleChecker = roleChecker;
        }

        public IActionResult All([FromQuery] CompanyListingQueryModel queryModel)
        {
            queryModel = companyService.All(
                queryModel.Name,
                queryModel.SpecializationId,
                queryModel.CountryId,
                queryModel.EmployeesCount,
                queryModel.IsPublic,
                queryModel.IsGovernmentOwned,
                queryModel.CurrentPage,
                queryModel.CompaniesPerPage);

            queryModel.Specializations = AcquireCachedSpecializations();
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

            var companyModel = new RegisterCompanyFormModel
            {
                Specializations = AcquireCachedSpecializations(),
                Countries = AcquireCachedCountries()
            };

            return View(companyModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Register(RegisterCompanyFormModel company)
        {
            var userId = User.Id();

            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(userId))
            {
                return BadRequest();
            }

            if (!specializationService.Exists(company.SpecializationId))
            {
                ModelState.AddModelError(nameof(company.SpecializationId), "Invalid option.");
            }

            if (!countryService.Exists(company.CountryId))
            {
                ModelState.AddModelError(nameof(company.CountryId), "Invalid option.");
            }

            if (!ModelState.IsValid)
            {
                company.Specializations = AcquireCachedSpecializations();
                company.Countries = AcquireCachedCountries();

                return View(company);
            }

            var result = companyService.Add(
                userId,
                company.Name,
                company.ImageUrl,
                company.WebsiteUrl,
                company.Founded,
                company.Description,
                company.RevenueUSD,
                company.CEO,
                company.EmployeesCount,
                company.IsPublic,
                company.IsGovernmentOwned,
                company.SpecializationId,
                company.CountryId,
                HttpContext.Request.Host.Value);

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

            TempData[GlobalMessageKey] = "Thank you for registering your company!";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(string id)
        {
            var company = companyService.GetDetailsModel(id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
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