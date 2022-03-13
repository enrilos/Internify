namespace Internify.Web.Areas.Company.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Internship;
    using Internify.Models.ViewModels.Country;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Company;
    using Services.Country;
    using Services.Internship;

    using static Common.WebConstants;

    public class InternshipController : CompanyController
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

        // each company can see all applications for their X internship/s.

        public IActionResult MyInternships([FromQuery] InternshipListingQueryModel queryModel)
        {
            // Prevent other copmaines from viewing internships of others.
            if (queryModel.CompanyId != companyService.GetIdByUserId(User.Id()))
            {
                return Unauthorized();
            }

            queryModel = internshipService.All(
                queryModel.Role,
                queryModel.IsPaid,
                queryModel.IsRemote,
                queryModel.CompanyId,
                queryModel.CountryId,
                queryModel.CurrentPage,
                queryModel.InternshipsPerPage);

            queryModel.Countries = AcquireCachedCountries();

            return View(queryModel);
        }

        public IActionResult Add()
        {
            var internshipModel = new AddInternshipFormModel
            {
                CompanyId = companyService.GetIdByUserId(User.Id()),
                Countries = AcquireCachedCountries()
            };

            return View(internshipModel);
        }

        [HttpPost]
        public IActionResult Add(AddInternshipFormModel internship)
        {
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

            if (!internship.IsRemote && internship.CountryId == null)
            {
                ModelState.AddModelError(nameof(internship.CountryId), "Non-remote internships require a country.");
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

            return RedirectToAction("Details", "Internship", new { id = internshipId });
        }

        public IActionResult Edit(string id)
        {
            if (!IsCurrentCompanyOwner(id))
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

        [HttpPost]
        public IActionResult Edit(EditInternshipFormModel internship)
        {
            if (!IsCurrentCompanyOwner(internship.Id))
            {
                return Unauthorized();
            }

            if (internship.IsPaid && internship.SalaryUSD == null)
            {
                ModelState.AddModelError(nameof(internship.SalaryUSD), "SalaryUSD is mandatory when internship is paid.");
            }

            if (!internship.IsPaid && internship.SalaryUSD != null)
            {
                internship.SalaryUSD = null;
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

            return RedirectToAction("Details", "Internship", new { id = internship.Id });
        }

        public IActionResult Delete(string id)
        {
            if (!IsCurrentCompanyOwner(id))
            {
                return Unauthorized();
            }

            var deleteResult = internshipService.Delete(id);

            if (!deleteResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully deleted internship.";

            var companyId = companyService.GetIdByUserId(User.Id());

            return RedirectToAction(nameof(MyInternships), new { companyId = companyId });
        }

        private bool IsCurrentCompanyOwner(string internshipId)
            => internshipService
                .IsInternshipOwnedByCompany(
                internshipId,
                companyService.GetIdByUserId(User.Id()));

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