namespace Internify.Web.Controllers
{
    using Common;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CompanyController : Controller
    {
        private readonly RoleChecker roleChecker;

        public CompanyController(RoleChecker roleChecker)
            => this.roleChecker = roleChecker;

        [Authorize]
        public IActionResult Register()
        {
            if (User.IsAdmin() || roleChecker.IsUserInAnyRole(User.Id()))
            {
                return BadRequest();
            }

            return View();
        }
    }
}