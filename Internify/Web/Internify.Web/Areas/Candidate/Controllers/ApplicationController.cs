namespace Internify.Web.Areas.Candidate.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Application;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Application;
    using Services.Data.Candidate;
    using Services.Data.Company;
    using Services.Data.Internship;

    using static Common.WebConstants;

    public class ApplicationController : CandidateControllerBase
    {
        private readonly IApplicationService applicationService;
        private readonly IInternshipService internshipService;
        private readonly ICompanyService companyService;
        private readonly ICandidateService candidateService;

        public ApplicationController(
            IApplicationService applicationService,
            IInternshipService internshipService,
            ICompanyService companyService,
            ICandidateService candidateService)
        {
            this.applicationService = applicationService;
            this.internshipService = internshipService;
            this.companyService = companyService;
            this.candidateService = candidateService;
        }

        public IActionResult MyApplications([FromQuery] MyApplicationListingQueryModel queryModel)
        {
            // Prevent other candidates from viewing applications of others.
            if (queryModel.CandidateId != candidateService.GetIdByUserId(User.Id()))
            {
                return Unauthorized();
            }

            // Check if candidate is already an intern.
            if (candidateService.IsCandidateAlreadyAnIntern(queryModel.CandidateId))
            {
                return BadRequest();
            }

            var applications = applicationService.GetCandidateApplications(
                queryModel.CandidateId,
                queryModel.Role,
                queryModel.CompanyId,
                queryModel.CurrentPage,
                queryModel.ApplicationsPerPage);

            applications.Companies = companyService.GetCompaniesSelectOptions();

            return View(applications);
        }

        public IActionResult ApplyForInternship(string id)
        {
            var candidateId = candidateService.GetIdByUserId(User.Id());

            // Check if candidate is already an intern.
            if (candidateService.IsCandidateAlreadyAnIntern(candidateId))
            {
                return BadRequest();
            }

            var applicationModel = new ApplicationFormModel
            {
                InternshipId = id,
                CandidateId = candidateId
            };

            return View(applicationModel);
        }

        [HttpPost]
        public IActionResult ApplyForInternship(ApplicationFormModel application)
        {
            // Check if candidate is already an intern.
            if (candidateService.IsCandidateAlreadyAnIntern(application.CandidateId))
            {
                return BadRequest();
            }

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
                return View(application);
            }

            var result = applicationService.Add(
                application.InternshipId,
                application.CandidateId,
                application.CoverLetter);

            if (result == null)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully applied for internship.";

            return RedirectToAction("Details", "Internship", new { id = application.InternshipId });
        }

        public IActionResult Edit(string id)
        {
            if (!IsCurrentCandidateOwner(id))
            {
                return Unauthorized();
            }

            if (!applicationService.Exists(id))
            {
                return NotFound();
            }

            var application = applicationService.GetEditModel(id);

            return View(application);
        }

        [HttpPost]
        public IActionResult Edit(ApplicationFormModel application)
        {
            if (!IsCurrentCandidateOwner(application.Id))
            {
                return Unauthorized();
            }

            if (!applicationService.Exists(application.Id))
            {
                return NotFound();
            }

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
                return View(application);
            }

            var editResult = applicationService.Edit(
                application.Id,
                application.CoverLetter);

            if (editResult == null)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully edited application.";

            return RedirectToAction(nameof(Details), new { id = application.Id });
        }

        public IActionResult Details(string id)
        {
            if (!IsCurrentCandidateOwner(id))
            {
                return Unauthorized();
            }

            var application = applicationService.GetDetailsModel(id);

            return View(application);
        }

        public IActionResult Delete(string id)
        {
            if (!IsCurrentCandidateOwner(id))
            {
                return Unauthorized();
            }

            var result = applicationService.Delete(id);

            if (!result)
            {
                return BadRequest();
            }

            var candidateId = candidateService.GetIdByUserId(User.Id());

            TempData[GlobalMessageKey] = "Successfully deleted application.";

            return RedirectToAction(nameof(MyApplications), new { candidateId = candidateId });
        }

        private bool IsCurrentCandidateOwner(string applicationId)
            => applicationService
                .IsApplicationOwnedByCandidate(
                applicationId,
                candidateService.GetIdByUserId(User.Id()));
    }
}