namespace Internify.Services.Administrator
{
    using Candidate;
    using Company;
    using Models.ViewModels.Administrator;
    using University;

    public class AdministratorService : IAdministratorService
    {
        private readonly ICandidateService candidateService;
        private readonly ICompanyService companyService;
        private readonly IUniversityService universityService;

        public AdministratorService(
            ICandidateService candidateService,
            ICompanyService companyService,
            IUniversityService universityService)
        {
            this.candidateService = candidateService;
            this.companyService = companyService;
            this.universityService = universityService;
        }

        public UserCountPerRoleViewModel GetUserCountPerRole()
            => new UserCountPerRoleViewModel
            {
                Candidates = candidateService.TotalCount(),
                Companies = companyService.TotalCount(),
                Universities = universityService.TotalCount(),
            };
    }
}