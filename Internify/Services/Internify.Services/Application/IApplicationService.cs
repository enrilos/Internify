namespace Internify.Services.Application
{
    using Models.InputModels.Application;
    using Models.InputModels.Internship;
    using Models.ViewModels.Application;

    public interface IApplicationService
    {
        public bool Add(
            string internshipId,
            string candidateId,
            string coverLetter);

        public string Edit(
            string id,
            string coverLetter);

        public bool HasCandidateApplied(
            string candidateId,
            string internshipId);

        public bool Exists(string id);

        public bool IsApplicationOwnedByCandidate(
            string applicationId,
            string candidateId);

        public bool DoesApplicationBelongToCompanyInternship(
            string applicationId,
            string companyId);

        public bool Delete(string id);

        public ApplicationDetailsViewModel GetDetailsModel(string id);

        public ApplicationForCompanyDetailsViewModel GetDetailsModelForCompany(string id);

        public ApplicationFormModel GetEditModel(string id);

        public InternshipApplicantListingQueryModel GetInternshipApplicants(
            string internshipId,
            string internshipRole,
            int currentPage,
            int applicantsPerPage);

        public MyApplicationListingQueryModel GetCandidateApplications(
            string candidateId,
            string role,
            string companyId,
            int currentPage,
            int applicationsPerPage);
    }
}