namespace Internify.Web.Controllers
{
    using Infrastructure.Extensions;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly RoleChecker roleChecker;

        public HomeController(
            ILogger<HomeController> logger,
            RoleChecker roleChecker)
        {
            this.logger = logger;
            this.roleChecker = roleChecker;
        }

        public IActionResult Index() => View();

        [Authorize]
        public IActionResult SelectRole()
        {
            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(User.Id()))
            {
                return BadRequest();
            }

            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}