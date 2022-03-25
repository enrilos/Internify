namespace Internify.Web.Areas.Company.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Internship;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Internify.Services.Specialization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Application;
    using Services.Company;
    using Services.Country;
    using Services.Internship;

    using static Common.WebConstants;

    public class InternshipController : CompanyControllerBase
    {
        private readonly IInternshipService internshipService;
        private readonly IApplicationService applicationService;
        private readonly ICompanyService companyService;
        private readonly ISpecializationService specializationService;
        private readonly ICountryService countryService;
        private readonly IMemoryCache cache;

        public InternshipController(
            IInternshipService internshipService,
            IApplicationService applicationService,
            ICompanyService companyService,
            ISpecializationService specializationService,
            ICountryService countryService,
            IMemoryCache cache)
        {
            this.internshipService = internshipService;
            this.applicationService = applicationService;
            this.companyService = companyService;
            this.specializationService = specializationService;
            this.countryService = countryService;
            this.cache = cache;
        }

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

        public IActionResult Applicants([FromQuery] InternshipApplicantListingQueryModel queryModel)
        {
            var companyId = companyService.GetIdByUserId(User.Id());

            if (!internshipService.IsInternshipOwnedByCompany(queryModel.InternshipId, companyId))
            {
                return Unauthorized();
            }

            queryModel = applicationService.GetInternshipApplicants(
                queryModel.ApplicantFirstName,
                queryModel.ApplicantLastName,
                queryModel.ApplicantSpecializationId,
                queryModel.ApplicantCountryId,
                queryModel.InternshipId,
                queryModel.InternshipRole,
                queryModel.CurrentPage,
                queryModel.ApplicantsPerPage);

            queryModel.Specializations = AcquireCachedSpecializations();
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

            if (internshipId == null)
            {
                return BadRequest();
            }

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