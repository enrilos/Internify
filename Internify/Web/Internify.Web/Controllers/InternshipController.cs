namespace Internify.Web.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Internship;
    using Internify.Models.ViewModels.Country;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Company;
    using Services.Country;
    using Services.Internship;

    using static Common.WebConstants;

    public class InternshipController : Controller
    {
        private readonly IInternshipService internshipService;
        private readonly ICompanyService companyService;
        private readonly ICountryService countryService;
        private readonly IMemoryCache cache;

        public InternshipController(
            IInternshipService internshipService,
            ICompanyService companyService,
            ICountryService countryService,
            IMemoryCache cache)
        {
            this.internshipService = internshipService;
            this.companyService = companyService;
            this.countryService = countryService;
            this.cache = cache;
        }

        [Authorize]
        public IActionResult All([FromQuery] InternshipListingQueryModel queryModel)
        {
            // filter
            // acquire companies (not cached OR cached for < 10 minutes?)
            // acquire cached countries

            return View();
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!companyService.IsCompanyByUserId(User.Id()))
            {
                return Unauthorized();
            }

            var internshipModel = new AddInternshipFormModel
            {
                CompanyId = companyService.GetIdByUserId(User.Id()),
                Countries = AcquireCachedCountries()
            };

            return View(internshipModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(AddInternshipFormModel internship)
        {
            if (!companyService.IsCompanyByUserId(User.Id()))
            {
                return Unauthorized();
            }

            if (!companyService.Exists(internship.CompanyId))
            {
                ModelState.AddModelError(nameof(internship.CompanyId), "Invalid option.");
            }

            if (internship.CountryId != null)
            {
                if (!countryService.Exists(internship.CountryId))
                {
                    ModelState.AddModelError(nameof(internship.CountryId), "Invalid option.");
                }
            }

            if (internship.IsPaid && internship.SalaryUSD == null)
            {
                ModelState.AddModelError(nameof(internship.SalaryUSD), "SalaryUSD is mandatory when internship is paid.");
            }

            if (!internship.IsPaid && internship.SalaryUSD != null)
            {
                ModelState.AddModelError(nameof(internship.SalaryUSD), "SalaryUSD is not required when internship is not paid.");
            }

            if (internship.IsRemote && internship.CountryId != null)
            {
                ModelState.AddModelError(nameof(internship.CountryId), "Remote internships do not require a country.");
            }

            if (!ModelState.IsValid)
            {
                internship.Countries = AcquireCachedCountries();

                return View(internship);
            }

            var internshipId = internshipService.Add(
                internship.CompanyId,
                internship.Role,
                internship.IsPaid,
                internship.SalaryUSD,
                internship.IsRemote,
                internship.Description,
                internship.CountryId);

            TempData[GlobalMessageKey] = "Internship published successfully.";

            return RedirectToAction(nameof(Details), new { id = internshipId });
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var isCurrentCompanyOwner = internshipService
                .IsInternshipOwnedByCompany(
                id,
                companyService.GetIdByUserId(User.Id()));

            if (!isCurrentCompanyOwner)
            {
                return Unauthorized();
            }

            var internship = internshipService.GetEditModel(id);

            if (internship == null)
            {
                return NotFound();
            }

            return View(internship);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditInternshipFormModel internship)
        {
            var isCurrentCompanyOwner = internshipService
                .IsInternshipOwnedByCompany(
                internship.Id,
                companyService.GetIdByUserId(User.Id()));

            if (!isCurrentCompanyOwner)
            {
                return Unauthorized();
            }

            if (internship.IsPaid && internship.SalaryUSD == null)
            {
                ModelState.AddModelError(nameof(internship.SalaryUSD), "SalaryUSD is mandatory when internship is paid.");
            }

            if (!internship.IsPaid && internship.SalaryUSD != null)
            {
                ModelState.AddModelError(nameof(internship.SalaryUSD), "SalaryUSD is not required when internship is not paid.");
            }

            if (!ModelState.IsValid)
            {
                return View(internship);
            }

            var editResult = internshipService.Edit(
                internship.Id,
                internship.IsPaid,
                internship.SalaryUSD,
                internship.Description);

            if (!editResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully edited internship.";

            return RedirectToAction(nameof(Details), new { id = internship.Id });
        }

        public IActionResult Details(string id)
        {
            return null;
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