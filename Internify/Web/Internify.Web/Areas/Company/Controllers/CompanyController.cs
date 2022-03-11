﻿namespace Internify.Web.Areas.Company.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Internify.Common.GlobalConstants;

    [Area(CompanyRoleName)]
    [Authorize(Roles = CompanyRoleName)]
    [Route($"{CompanyRoleName}/[controller]/[action]/{{id?}}")]
    public class CompanyController : Controller
    {
    }
}