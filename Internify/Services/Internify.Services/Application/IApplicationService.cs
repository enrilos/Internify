namespace Internify.Services.Application
{
    using Models.InputModels.Application;
    using Models.InputModels.Internship;
    using Models.ViewModels.Application;

    public interface IApplicationService
    {
        bool Add(
           string internshipId,
           string candidateId,
           string coverLetter);

        string Edit(
           string id,
           string coverLetter);

        bool HasCandidateApplied(
           string candidateId,
           string internshipId);

        bool Exists(string id);

        bool IsApplicationOwnedByCandidate(
           string applicationId,
           string candidateId);

        bool DoesApplicationBelongToCompanyInternship(
           string applicationId,
           string companyId);

        bool Delete(string id);

        ApplicationDetailsViewModel GetDetailsModel(string id);

        ApplicationForCompanyDetailsViewModel GetDetailsModelForCompany(string id);

        ApplicationFormModel GetEditModel(string id);

        InternshipApplicantListingQueryModel GetInternshipApplicants(
           string applicantFirstName,
           string applicantLastName,
           string applicantSpecializationId,
           string applicantCountryId,
           string internshipId,
           string internshipRole,
           int currentPage,
           int applicantsPerPage);

        MyApplicationListingQueryModel GetCandidateApplications(
           string candidateId,
           string role,
           string companyId,
           int currentPage,
           int applicationsPerPage);
    }
}