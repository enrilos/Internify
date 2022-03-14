namespace Internify.Web.Areas.Candidate.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Internify.Common.GlobalConstants;

    [Area(CandidateRoleName)]
    [Authorize(Roles = CandidateRoleName)]
    [Route($"{CandidateRoleName}/[controller]/[action]/{{id?}}")]
    public class CandidateControllerBase : Controller
    {
    }
}