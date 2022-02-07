namespace Internify.Web.Areas.Company.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    [Area(CompanyRoleName)]
    [Authorize(Roles = CompanyRoleName)]
    [Route($"{CompanyRoleName}/[controller]/[action]/{{id?}}")]
    public abstract class CompanyController : Controller
    {
    }
}
