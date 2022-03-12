namespace Internify.Web.Areas.Candidate.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Application;
    using Microsoft.AspNetCore.Mvc;
    using Services.Application;
    using Services.Candidate;
    using Services.Internship;

    public class ApplicationController : CandidateController
    {
        private readonly IApplicationService applicationService;
        private readonly IInternshipService internshipService;
        private readonly ICandidateService candidateService;

        public ApplicationController(
            IApplicationService applicationService,
            IInternshipService internshipService,
            ICandidateService candidateService)
        {
            this.applicationService = applicationService;
            this.internshipService = internshipService;
            this.candidateService = candidateService;
        }

        public IActionResult MyApplications(string candidateId)
        {
            // applications listing and other data;

            // TODO...

            var obj = new { };

            return View(obj);
        }

        public IActionResult ApplyForInternship(string id)
        {
            var candidateId = candidateService.GetIdByUserId(User.Id());

            var applicationModel = new AddApplicationFormModel
            {
                InternshipId = id,
                CandidateId = candidateId
            };

            return View(applicationModel);
        }

        [HttpPost]
        public IActionResult ApplyForInternship(AddApplicationFormModel application)
        {
            if (!internshipService.Exists(application.InternshipId))
            {
                ModelState.AddModelError(nameof(application.InternshipId), "Invalid option.");
            }

            if (!candidateService.Exists(application.CandidateId))
            {
                ModelState.AddModelError(nameof(application.CandidateId), "Invalid option.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = applicationService.Add(
                application.InternshipId,
                application.CandidateId,
                application.CoverLetter);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", "Internship", new { id = application.InternshipId });
        }
    }
}