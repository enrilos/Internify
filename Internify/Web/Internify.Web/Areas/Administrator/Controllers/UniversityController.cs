namespace Internify.Web.Areas.Administrator.Controllers
{
    using Services.University;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    public class UniversityController : AdministratorController
    {
        private readonly IUniversityService universityService;

        public UniversityController(IUniversityService universityService)
        {
            this.universityService = universityService;
        }

        public IActionResult Delete(string id)
        {
            var deleteResult = universityService.Delete(id);

            if (!deleteResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully deleted university.";

            return RedirectToAction("All", "University");
        }
    }
}
