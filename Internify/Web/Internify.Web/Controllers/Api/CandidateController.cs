namespace Internify.Web.Controllers.Api
{
    using Internify.Services.Candidate;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService candidateService;

        public CandidateController(ICandidateService candidateService)
            => this.candidateService = candidateService;

        [HttpGet("{id}")]
        public ActionResult<string> GetEmail(string id) => candidateService.GetEmail(id);
    }
}