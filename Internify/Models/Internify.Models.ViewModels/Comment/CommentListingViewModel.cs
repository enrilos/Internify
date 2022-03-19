namespace Internify.Models.ViewModels.Comment
{
    public class CommentListingViewModel
    {
        public string CandidateFullName { get; init; }

        public string CandidateId { get; init; }

        public string Content { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}