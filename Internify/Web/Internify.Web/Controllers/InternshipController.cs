namespace Internify.Web.Controllers
{
    using Internify.Models.InputModels.Internship;
    using Internify.Models.ViewModels.Country;
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

        public IActionResult All([FromQuery] InternshipListingQueryModel queryModel)
        {
            queryModel = internshipService.All(
                  queryModel.Role,
                  queryModel.IsPaid,
                  queryModel.IsRemote,
                  queryModel.CompanyId,
                  queryModel.CountryId,
                  queryModel.CurrentPage,
                  queryModel.InternshipsPerPage);

            queryModel.Companies = companyService.GetCompaniesSelectOptions();
            queryModel.Countries = AcquireCachedCountries();

            return View(queryModel);
        }

        public IActionResult Details(string id)
        {
            var internship = internshipService.GetDetailsModel(id);

            if (internship == null)
            {
                return NotFound();
            }

            return View(internship);
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