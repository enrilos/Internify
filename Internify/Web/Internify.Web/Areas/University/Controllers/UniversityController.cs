namespace Internify.Web.Areas.University.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    [Area(UniversityRoleName)]
    [Authorize(Roles = UniversityRoleName)]
    [Route($"{UniversityRoleName}/[controller]/[action]/{{id?}}")]
    public abstract class UniversityController : Controller
    {
    }
}