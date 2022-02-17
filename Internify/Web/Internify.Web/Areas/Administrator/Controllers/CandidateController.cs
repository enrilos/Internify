namespace Internify.Web.Areas.Administrator.Controllers
{
    using Services.Candidate;
    using Microsoft.AspNetCore.Mvc;

    using static Common.WebConstants;

    public class CandidateController : AdministratorController
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

            // when using return View
            // nameof(VIEWNAME) is required.
            // Otherwise, an expection is thrown despite the fact that asp net MVC standards in this project are kept.
            return RedirectToAction("All", "Candidate");
        }
    }
}