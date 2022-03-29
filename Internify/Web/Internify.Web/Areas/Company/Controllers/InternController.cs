namespace Internify.Web.Areas.Company.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Intern;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Candidate;
    using Services.Data.Company;

    using static Common.WebConstants;

    public class InternController : CompanyControllerBase
    {
        private readonly ICandidateService candidateService;
        private readonly ICompanyService companyService;

        public InternController(
            ICandidateService candidateService,
            ICompanyService companyService)
        {
            this.candidateService = candidateService;
            this.companyService = companyService;
        }

        public IActionResult MyInterns([FromQuery] InternListingQueryModel queryModel)
        {
            var currentCompanyId = companyService.GetIdByUserId(User.Id());

            // Prevent other companies from viewing interns of others.
            if (queryModel.CompanyId != currentCompanyId)
            {
                return Unauthorized();
            }

            var interns = candidateService.GetCandidatesByCompany(
                queryModel.CompanyId,
                queryModel.FirstName,
                queryModel.LastName,
                queryModel.InternshipRole,
                queryModel.CurrentPage,
                queryModel.InternsPerPage);

            return View(interns);
        }

        public IActionResult Remove(string candidateId)
        {
            var companyId = companyService.GetIdByUserId(User.Id());

            if (!candidateService.IsCandidateInCompany(candidateId, companyId))
            {
                return BadRequest();
            }

            var result = candidateService.RemoveFromCompany(candidateId);

            if (!result)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully removed intern from company.";

            return RedirectToAction(nameof(MyInterns), new { companyId = companyId });
        }
    }
}