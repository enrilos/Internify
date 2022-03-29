namespace Internify.Web.Areas.Administrator.Controllers
{
    using Services.Data.Administrator;
    using Microsoft.AspNetCore.Mvc;

    public class AdministratorController : AdministratorControllerBase
    {
        private readonly IAdministratorService administratorService;

        public AdministratorController(IAdministratorService administratorService)
            => this.administratorService = administratorService;

        public IActionResult Dashboard()
        {
            var data = administratorService.GetUserCountPerRole();

            return View(data);
        }
    }
}