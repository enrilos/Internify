﻿namespace Internify.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    [Area(AdministratorRoleName)]
    [Authorize(Roles = AdministratorRoleName)]
    [Route($"{AdministratorRoleName}/[controller]/[action]/{{id?}}")]
    public class AdministratorController : Controller
    {
    }
}