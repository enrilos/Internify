namespace Internify.Web.Controllers
{
    using Infrastructure;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UniversityController : Controller
    {
        private readonly RoleChecker roleChecker;

        // TODO: Consider making ApiController that works with dynamic search bar functionality here.
        // Search university by name dynamically.

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