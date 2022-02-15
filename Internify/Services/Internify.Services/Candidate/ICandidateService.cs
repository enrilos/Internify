namespace Internify.Services.Candidate
{
    using Data.Models.Enums;
    using Models.InputModels.Candidate;
    using Models.ViewModels.Candidate;

    public interface ICandidateService
    {
        public bool IsCandidate(string userId);

        public string GetIdByUserId(string userId);

        public string Add(
            string userId,
            string firstName,
            string lastName,
            string description,
            string imageUrl,
            string websiteUrl,
            DateTime birthDate,
            Gender gender,
            string specializationId,
            string countryId,
            string hostName);

        public CandidateDetailsViewModel Get(string id);

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
