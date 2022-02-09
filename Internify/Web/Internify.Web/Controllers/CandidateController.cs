namespace Internify.Web.Controllers
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Candidate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Candidate;
    using Services.Country;
    using Services.Specialization;

    using static Common.WebConstants;

    public class CandidateController : Controller
    {
        private readonly InternifyDbContext data;
        private readonly RoleChecker roleChecker;
        private readonly IMapper mapper;
        private readonly ICountryService countryService;
        private readonly ISpecializationService specializationService;
        private readonly ICandidateService candidateService;

        public CandidateController(
            InternifyDbContext data,
            RoleChecker roleChecker,
            IMapper mapper,
            ICountryService countryService,
            ISpecializationService specializationService,
            ICandidateService candidateService)
        {
            this.data = data;
            this.roleChecker = roleChecker;
            this.mapper = mapper;
            this.countryService = countryService;
            this.specializationService = specializationService;
            this.candidateService = candidateService;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(User.Id()))
            {
                return BadRequest();
            }

            var candidateModel = new BecomeCandidateFormModel
            {
                Countries = countryService.All(),
                Specializations = specializationService.All()
            };

            return View(candidateModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeCandidateFormModel candidate)
        {
            var userId = User.Id();

            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(userId))
            {
                return BadRequest();
            }

            if (!countryService.Exists(candidate.CountryId)
                || !specializationService.Exists(candidate.SpecializationId))
            {
                ModelState.AddModelError(nameof(candidate.CountryId), "Invalid data.");
            }

            if (!ModelState.IsValid)
            {
                candidate.Countries = countryService.All();
                candidate.Specializations = specializationService.All();

                return View(candidate);
            }

            var candidateData = mapper.Map<Candidate>(candidate);

            candidateData.UserId = userId;

            if (candidateData.ImageUrl == null)
            {
                candidateData.ImageUrl = "https://freesvg.org/img/abstract-user-flat-1.png";
            }

            data.Candidates.Add(candidateData);
            data.SaveChanges();

            // TODO: Implement post-action messages.
            TempData[GlobalMessageKey] = "Thank you for becoming a candidate!";

            return RedirectToAction(nameof(All));
        }

        public IActionResult All()
        {
            var candidates = candidateService
                .All()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName);

            return View(candidates);
        }
    }
}