namespace Internify.Web.Areas.Company.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Application;
    using Services.Data.Candidate;
    using Services.Data.Company;
    using Services.Data.Internship;

    using static Common.WebConstants;

    public class ApplicationController : CompanyControllerBase
    {
        private readonly IApplicationService applicationService;
        private readonly ICandidateService candidateService;
        private readonly IInternshipService internshipService;
        private readonly ICompanyService companyService;

        public ApplicationController(
            IApplicationService applicationService,
            ICandidateService candidateService,
            IInternshipService internshipService,
            ICompanyService companyService)
        {
            this.applicationService = applicationService;
            this.candidateService = candidateService;
            this.internshipService = internshipService;
            this.companyService = companyService;
        }

        public IActionResult Details(string id)
        {
            var companyId = companyService.GetIdByUserId(User.Id());

            if (!applicationService.DoesApplicationBelongToCompanyInternship(id, companyId))
            {
                return Unauthorized();
            }

            var applicationForCompany = applicationService.GetDetailsModelForCompany(id);

            return View(applicationForCompany);
        }

        public IActionResult Approve(
            string candidateId,
            string internshipId)
        {
            var companyId = companyService.GetIdByUserId(User.Id());

            if (candidateService.IsCandidateAlreadyAnIntern(candidateId))
            {
                return BadRequest();
            }

            if (!candidateService.Exists(candidateId)
                || !internshipService.Exists(internshipId))
            {
                return NotFound();
            }

            var internshipRole = internshipService.GetRoleById(internshipId);
            var internshipDeleteResult = internshipService.Delete(internshipId);

            if (!internshipDeleteResult)
            {
                return BadRequest();
            }

            var approvalResult = companyService.AddCandidateToInterns(candidateId, companyId, internshipRole);

            if (!approvalResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully employed candidate as an intern.";

            return RedirectToAction("Details", "Company", new { id = companyId });
        }
    }
}