namespace Internify.Web.Areas.University.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.University;
    using Internify.Models.ViewModels.Country;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Data.CandidateUniversity;
    using Services.Data.Country;
    using Services.Data.University;

    using static Internify.Common.GlobalConstants;
    using static Web.Common.WebConstants;

    [Area(UniversityRoleName)]
    [Authorize(Roles = UniversityRoleName)]
    [Route($"{UniversityRoleName}/[controller]/[action]/{{id?}}")]
    public class UniversityController : Controller
    {
        private readonly IUniversityService universityService;
        private readonly ICandidateUniversityService candidateUniversityService;
        private readonly ICountryService countryService;
        private readonly IMemoryCache cache;

        public UniversityController(
            IUniversityService universityService,
            ICandidateUniversityService candidateUniversityService,
            ICountryService countryService,
            IMemoryCache cache)
        {
            this.universityService = universityService;
            this.candidateUniversityService = candidateUniversityService;
            this.countryService = countryService;
            this.cache = cache;
        }

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

            return RedirectToAction("Details", "University", new { id = university.Id });
        }

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

            return RedirectToAction("Details", "University", new { id = universityId });
        }

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

            return RedirectToAction("Details", "University", new { id = universityId });
        }

        private bool IsTheSameUniversity(string id)
            => universityService
            .GetIdByUserId(User.Id()) == id;

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