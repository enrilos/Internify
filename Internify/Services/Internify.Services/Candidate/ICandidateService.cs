namespace Internify.Services.Candidate
{
    using Data.Models.Enums;
    using Models.InputModels.Intern;
    using Models.InputModels.Candidate;
    using Models.ViewModels.Candidate;

    public interface ICandidateService
    {
        bool IsCandidate(string id);

        bool IsCandidateByUserId(string userId);

        string GetIdByUserId(string userId);

        int TotalCount();

        string Add(
            string userId,
            string firstName,
            string lastName,
            string imageUrl,
            string websiteUrl,
            string description,
            DateTime birthDate,
            Gender gender,
            string specializationId,
            string countryId,
            string hostName);

        bool Edit(
            string id,
            string firstName,
            string lastName,
            string imageUrl,
            string websiteUrl,
            string description,
            DateTime birthDate,
            Gender gender,
            bool isAvailable,
            string specializationId,
            string countryId);

        bool Delete(string id);

        string GetEmail(string id);

        bool IsCandidateAlreadyAnIntern(string id);

        bool IsCandidateInCompany(
            string candidateId,
            string companyId);

        bool RemoveFromCompany(string candidateId);

        bool Exists(string id);

        CandidateDetailsViewModel GetDetailsModel(string id);

        EditCandidateFormModel GetEditModel(string id);

        CandidateListingQueryModel All(
            string firstName,
            string lastName,
            string specializationId,
            string countryId,
            bool isAvailable,
            int currentPage,
            int candidatesPerPage);

        InternListingQueryModel GetCandidatesByCompany(
            string companyId,
            string firstName,
            string lastName,
            string internshipRole,
            int currentPage,
            int internsPerPage);
    }
}