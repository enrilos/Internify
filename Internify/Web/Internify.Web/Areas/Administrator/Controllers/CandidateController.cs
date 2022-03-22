namespace Internify.Web.Areas.Administrator.Controllers
{
    using Services.Candidate;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    public class CandidateController : AdministratorControllerBase
    {
        private readonly ICandidateService candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            this.candidateService = candidateService;
        }

        public IActionResult Delete(string id)
        {
            var deleteResult = candidateService.Delete(id);

            if (!deleteResult)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully deleted candidate.";

            return RedirectToAction("All", "Candidate");
        }
    }
}