namespace Internify.Services.Candidate
{
    using Data.Models.Enums;
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

        public IEnumerable<CandidateListingViewModel> All(
            string fullName = "",
            string specializationId = "",
            string countryId = "",
            bool isAvailable = true,
            int currentPage = 1,
            int candidatesPerPage = 2);
    }
}
