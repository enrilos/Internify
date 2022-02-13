namespace Internify.Web.Common
{
    using Services.Candidate;
    using Services.Company;
    using Services.University;

    public class RoleChecker
    {
        private readonly ICandidateService candidateService;
        private readonly ICompanyService companyService;
        private readonly IUniversityService universityService;

        public RoleChecker(
            ICandidateService candidateService,
            ICompanyService companyService,
            IUniversityService universityService)
        {
            this.candidateService = candidateService;
            this.companyService = companyService;
            this.universityService = universityService;
        }

        public bool IsUserInAnyRole(string userId)
            => candidateService.IsCandidate(userId)
            || companyService.IsCompany(userId)
            || universityService.IsUniversity(userId);
    }
}
