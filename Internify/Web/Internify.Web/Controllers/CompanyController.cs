namespace Internify.Web.Controllers
{
    using Common;
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Company;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Company;
    using Services.Country;
    using Services.Specialization;

    using static Common.WebConstants;

    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly ISpecializationService specializationService;
        private readonly ICountryService countryService;
        private readonly IMemoryCache cache;
        private readonly RoleChecker roleChecker;

        public CompanyController(
            ICompanyService companyService,
            ISpecializationService specializationService,
            ICountryService countryService,
            IMemoryCache cache,
            RoleChecker roleChecker)
        {
            this.companyService = companyService;
            this.specializationService = specializationService;
            this.countryService = countryService;
            this.cache = cache;
            this.roleChecker = roleChecker;
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

            var companyId = companyService.Add(
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

            TempData[GlobalMessageKey] = "Thank you for registering your company!";

            return RedirectToAction(nameof(Details), new { id = companyId });
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

        [Authorize]
        public IActionResult Edit(string id)
        {
            if (!IsTheSameCompany(id))
            {
                return Unauthorized();
            }

            var company = companyService.GetEditModel(id);

            if (company == null)
            {
                return NotFound();
            }

            company.Specializations = AcquireCachedSpecializations();
            company.Countries = AcquireCachedCountries();

            return View(company);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditCompanyFormModel company)
        {
            if (!IsTheSameCompany(company.Id))
            {
                return Unauthorized();
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

            var editResult = companyService.Edit(
                company.Id,
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

            if (!editResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully edited company.";

            return RedirectToAction(nameof(Details), new { id = company.Id });
        }

        private bool IsTheSameCompany(string id)
            => companyService
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