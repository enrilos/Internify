namespace Internify.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Internify.Common.GlobalConstants;

    [Area(AdministratorRoleName)]
    [Authorize(Roles = AdministratorRoleName)]
    [Route($"{AdministratorRoleName}/[controller]/[action]/{{id?}}")]
    public class AdministratorControllerBase : Controller
    {
    }
}