namespace Internify.Services.Candidate
{
    using Data.Models.Enums;
    using Models.InputModels.Candidate;
    using Models.ViewModels.Candidate;

    public interface ICandidateService
    {
        public bool IsCandidate(string id);

        public bool IsCandidateByUserId(string userId);

        public string GetIdByUserId(string userId);

        public string Add(
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

        public bool Edit(
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

        public bool Delete(string id);

        public CandidateDetailsViewModel GetDetailsModel(string id);

        public EditCandidateFormModel GetEditModel(string id);

        public CandidateListingQueryModel All(
            string firstName,
            string lastName,
            string specializationId,
            string countryId,
            bool isAvailable,
            int currentPage,
            int candidatesPerPage);
    }
}