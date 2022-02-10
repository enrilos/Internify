namespace Internify.Services.Candidate
{
    using Models.ViewModels.Candidate;

    public interface ICandidateService
    {
        public bool IsCandidate(string userId);

        public string GetIdByUserId(string userId);

        public IEnumerable<CandidateListingViewModel> All(
            string fullName = "",
            bool isAvailable = true,
            string specializationId = "",
            string countryId = "");
    }
}
