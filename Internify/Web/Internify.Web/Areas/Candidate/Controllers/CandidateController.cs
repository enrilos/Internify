namespace Internify.Web.Areas.Candidate.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    [Area(CandidateRoleName)]
    [Authorize(Roles = CandidateRoleName)]
    [Route($"{CandidateRoleName}/[controller]/[action]/{{id?}}")]
    public abstract class CandidateController : Controller
    {
    }
}