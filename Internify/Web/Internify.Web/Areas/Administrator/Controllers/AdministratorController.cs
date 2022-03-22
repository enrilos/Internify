namespace Internify.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Candidate;
    using Services.Company;
    using Services.University;

    public class AdministratorController : AdministratorControllerBase
    {
        private readonly ICandidateService candidateService;
        private readonly ICompanyService companyService;
        private readonly IUniversityService universityService;

        public AdministratorController(
            ICandidateService candidateService,
            ICompanyService companyService,
            IUniversityService universityService)
        {
            this.candidateService = candidateService;
            this.companyService = companyService;
            this.universityService = universityService;
        }

        public IActionResult Dashboard()
        {
            // make dashboard  btn dropdown? with options:
            // users (all)
            // internships (all)

            var data = new UserCountPerRoleViewModel
            {
                Candidates = candidateService.TotalCount(),
                Companies = companyService.TotalCount(),
                Universities = universityService.TotalCount(),
            };

            return View(data);
        }
    }
}