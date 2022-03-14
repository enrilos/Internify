namespace Internify.Web.Areas.Company.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using static Internify.Common.GlobalConstants;

    [Area(CompanyRoleName)]
    [Authorize(Roles = CompanyRoleName)]
    [Route($"{CompanyRoleName}/[controller]/[action]/{{id?}}")]
    public class CompanyControllerBase : Controller
    {
    }
}
