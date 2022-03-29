namespace Internify.Web.Common
{
    using Services.Data.Candidate;
    using Services.Data.Company;
    using Services.Data.University;

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
            => candidateService.IsCandidateByUserId(userId)
            || companyService.IsCompanyByUserId(userId)
            || universityService.IsUniversityByUserId(userId);
    }
}
