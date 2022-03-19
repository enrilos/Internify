namespace Internify.Services.Review
{
    public interface IReviewService
    {
        bool Add(
            string candidateId,
            string companyId,
            string title,
            int rating,
            string content);
    }
}