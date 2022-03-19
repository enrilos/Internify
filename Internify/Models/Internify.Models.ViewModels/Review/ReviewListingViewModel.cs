namespace Internify.Models.ViewModels.Review
{
    public class ReviewListingViewModel
    {
        public string Id { get; init; }

        public string CandidateFullName { get; init; }

        public string CandidateId { get; init; }

        public string CompanyName { get; init; }

        public string CompanyId { get; init; }

        public string Title { get; init; }

        public int Rating { get; init; }

        public string Content { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime? ModifiedOn { get; init; }
    }
}