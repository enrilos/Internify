﻿namespace Internify.Web.Areas.Company.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Intern;
    using Microsoft.AspNetCore.Mvc;
    using Services.Candidate;
    using Services.Company;

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

        public IActionResult RemoveIntern(string candidateId)
        {
            var companyId = companyService.GetIdByUserId(User.Id());

            // TODO..
            // Remove candidate from intern list.
            // Set Candidate - CompanyId and InternshipRole to null.

            return RedirectToAction(nameof(MyInterns), new { companyId = companyId });
        }
    }
}