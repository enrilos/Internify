namespace Internify.Web.Controllers
{
    using Data;
    using Data.Models;
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Candidate;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Candidate;
    using Web.Infrastructure;
    using static Common.WebConstants;

    public class CandidateController : Controller
    {
        private readonly InternifyDbContext data;
        private readonly RoleChecker roleChecker;
        private readonly ICandidateService candidateService;

        // TODO: Add services (business logic). DB should not be coupled with Web.
        // Initialize AutoMapper.
        public CandidateController(
            InternifyDbContext data,
            RoleChecker roleChecker,
            ICandidateService candidateService)
        {
            this.data = data;
            this.roleChecker = roleChecker;
            this.candidateService = candidateService;
        }

        [Authorize]
        public IActionResult Become()
        {
            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(User.Id()))
            {
                return BadRequest();
            }

            // This is why you need Services + AutoMapper.
            var candidateModel = new BecomeCandidateFormModel
            {
                Specializations = data.Specializations
                .Select(x => new SpecializationListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList(),
                Countries = data.Countries
                .Select(x => new CountryListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToList(),
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

            if (!ModelState.IsValid)
            {
                // Array items must be fetched again here.
                // (specializations, countries)
                return View(candidate);
            }

            // TODO: Use AutoMapper.
            var candidateData = new Candidate
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                UserId = userId,
                ImageUrl =  candidate.ImageUrl,
                WebsiteUrl = candidate.WebsiteUrl,
                BirthDate = candidate.BirthDate,
                Gender = candidate.Gender,
                SpecializationId = candidate.SpecializationId,
                CountryId = candidate.CountryId
            };

            data.Candidates.Add(candidateData);
            data.SaveChanges();

            // TODO: Implement post-action messages.
            TempData[GlobalMessageKey] = "Thank you for becoming a candidate!";

            return RedirectToAction("Index", "Home");
        }
    }
}