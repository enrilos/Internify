namespace Internify.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CompanyController : Controller
    {
        [Authorize]
        public IActionResult Register() => View();
    }
}