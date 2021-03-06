namespace Internify.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Candidate;

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