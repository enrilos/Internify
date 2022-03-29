namespace Internify.Services.Data.Review
{
    using Models.InputModels.Review;

    public interface IReviewService
    {
        bool Add(
            string candidateId,
            string companyId,
            string title,
            int rating,
            string content);

        ReviewListingQueryModel CompanyReviews(
            string companyId,
            string title,
            int? rating,
            int currentPage,
            int reviewsPerPage);

        ReviewListingQueryModel CandidateReviews(
            string candidateId,
            string companyId,
            string title,
            int? rating,
            int currentPage,
            int reviewsPerPage);
    }
}