namespace Internify.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CandidateController : Controller
    {
        [Authorize]
        public IActionResult Become() => View();
    }
}