namespace Internify.Services.Review
{
    using Ganss.XSS;
    using Internify.Data;
    using Internify.Data.Models;

    public class ReviewService : IReviewService
    {
        private readonly InternifyDbContext data;

        public ReviewService(InternifyDbContext data)
            => this.data = data;

        public bool Add(
            string candidateId,
            string companyId,
            string title,
            int rating,
            string content)
        {
            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(content);

            var review = new Review
            {
                CandidateId = candidateId,
                CompanyId = companyId,
                Title = title.Trim(),
                Content = sanitizedContent.Trim(),
                Rating = rating
            };

            data.Reviews.Add(review);

            var result = data.SaveChanges();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public bool HasCandidateReviewedCompany(
            string candidateId,
            string companyId)
            => data
            .Reviews
            .Any(x =>
            x.CandidateId == candidateId
            && x.CompanyId == companyId
            && !x.IsDeleted);
    }
}