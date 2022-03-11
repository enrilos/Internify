namespace Internify.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/[controller]")]
    public class ApiController : ControllerBase
    {
        protected const string Id = "{id}";
    }
}