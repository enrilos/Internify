namespace Internify.Web.Areas.Company.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services.Application;
    using Services.Company;

    public class ApplicationController : CompanyControllerBase
    {
        private readonly IApplicationService applicationService;
        private readonly ICompanyService companyService;

        public ApplicationController(
            IApplicationService applicationService,
            ICompanyService companyService)
        {
            this.applicationService = applicationService;
            this.companyService = companyService;
        }

        // only one candidate can be approved for an internship. After being approved, they become part of the company's interns.
        // Moreover, the internship should be deleted. This will also delete all its applications.
        // approve button in application details

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

        public IActionResult Approve(string candidateId) => null;
    }
}