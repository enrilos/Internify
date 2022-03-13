namespace Internify.Services.Application
{
    using Models.InputModels.Application;

    public interface IApplicationService
    {
        public bool Add(
            string internshipId,
            string candidateId,
            string coverLetter);

        public bool HasCandidateApplied(
            string candidateId,
            string internshipId);

        public MyApplicationListingQueryModel GetCandidateApplications(
            string candidateId,
            string role,
            string companyId,
            int currentPage,
            int applicationsPerPage);
    }
}