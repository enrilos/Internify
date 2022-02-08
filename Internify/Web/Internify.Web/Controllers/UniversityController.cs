namespace Internify.Web.Controllers
{
    using Infrastructure;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UniversityController : Controller
    {
        private readonly RoleChecker roleChecker;

        public UniversityController(RoleChecker roleChecker)
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