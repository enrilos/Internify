namespace Internify.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 400: return View("BadRequest");
                case 401: return View("Unauthorized");
                case 404: return View("NotFound");
                default: return RedirectToAction("Error", "Home");
            }
        }
    }
}