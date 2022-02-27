﻿namespace Internify.Web.Controllers
{
    using Common;
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.University;
    using Internify.Models.ViewModels.Country;
    using Services.University;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Country;

    using static Common.WebConstants;

    public class UniversityController : Controller
    {
        private readonly IUniversityService universityService;
        private readonly ICountryService countryService;
        private readonly IMemoryCache cache;
        private readonly RoleChecker roleChecker;

        public UniversityController(
            IUniversityService universityService,
            ICountryService countryService,
            IMemoryCache cache,
            RoleChecker roleChecker)
        {
            this.universityService = universityService;
            this.countryService = countryService;
            this.cache = cache;
            this.roleChecker = roleChecker;
        }

        public IActionResult All()
        {
            var universities = universityService.All();

            return View(universities);
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
                university.Description,
                university.CountryId);

            if (!editResult)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        private bool IsTheSameUniversity(string id)
            => universityService
            .GetIdByUserId(User?.Id()) == id;

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