namespace Internify.Models.ViewModels.University
{
    using Internify.Models.ViewModels.Candidate;

    public class UniversityListingViewModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string WebsiteUrl { get; init; }

        public string Description { get; init; }

        public string Country { get; init; }

        public ICollection<CandidateListingViewModel> Alumni { get; init; } = new List<CandidateListingViewModel>();
    }
}