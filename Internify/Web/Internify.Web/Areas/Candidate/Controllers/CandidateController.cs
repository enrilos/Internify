namespace Internify.Web.Areas.Candidate.Controllers
{
    using Services.Candidate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    [Area(CandidateRoleName)]
    [Authorize(Roles = CandidateRoleName)]
    [Route($"{CandidateRoleName}/[controller]/[action]/{{id?}}")]
    public class CandidateController : Controller
    {
        private readonly ICandidateService candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            this.candidateService = candidateService;
        }

        public IActionResult Edit(string id)
        {
            var candidate = candidateService.Get(id);

            return View(candidate);
        }

        [HttpPost]
        public IActionResult Edit(object candidate)
        {
            return null;
        }
    }
}