namespace Internify.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Company;

    using static Common.WebConstants;

    public class CompanyController : AdministratorControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        public IActionResult Delete(string id)
        {
            var deleteResult = companyService.Delete(id);

            if (!deleteResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully deleted company and its affiliated records.";

            return RedirectToAction("All", "Company");
        }
    }
}