namespace Internify.Web.Areas.Candidate.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Candidate;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Candidate;
    using Services.Country;
    using Services.Specialization;

    using static Common.WebConstants;

    public class CandidateController : CandidateControllerBase
    {
        private readonly ICandidateService candidateService;
        private readonly ISpecializationService specializationService;
        private readonly ICountryService countryService;
        private readonly IMemoryCache cache;

        public CandidateController(
            ICandidateService candidateService,
            ISpecializationService specializationService,
            ICountryService countryService,
            IMemoryCache cache)
        {
            this.candidateService = candidateService;
            this.specializationService = specializationService;
            this.countryService = countryService;
            this.cache = cache;
        }

        public IActionResult Edit(string id)
        {
            if (!IsTheSameCandidate(id))
            {
                return Unauthorized();
            }

            var candidate = candidateService.GetEditModel(id);

            if (candidate == null)
            {
                return NotFound();
            }

            candidate.Specializations = AcquireCachedSpecializations();
            candidate.Countries = AcquireCachedCountries();

            return View(candidate);
        }

        [HttpPost]
        public IActionResult Edit(EditCandidateFormModel candidate)
        {
            if (!IsTheSameCandidate(candidate.Id))
            {
                return Unauthorized();
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

            var editResult = candidateService.Edit(
                candidate.Id,
                candidate.FirstName,
                candidate.LastName,
                candidate.ImageUrl,
                candidate.WebsiteUrl,
                candidate.Description,
                candidate.BirthDate,
                candidate.Gender,
                candidate.IsAvailable,
                candidate.SpecializationId,
                candidate.CountryId);

            if (!editResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully edited candidate.";

            return RedirectToAction("Details", "Candidate", new { id = candidate.Id });
        }

        private bool IsTheSameCandidate(string id)
            => candidateService
            .GetIdByUserId(User.Id()) == id;

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