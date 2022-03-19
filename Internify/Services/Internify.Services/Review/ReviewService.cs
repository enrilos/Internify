namespace Internify.Services.Review
{
    using Data;
    using Data.Models;
    using Ganss.XSS;
    using Models.InputModels.Review;
    using Models.ViewModels.Review;

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

        public ReviewListingQueryModel CompanyReviews(
           string companyId,
           string title,
           int? rating,
           int currentPage,
           int reviewsPerPage)
        {
            var reviewsQuery = data
                .Reviews
                .Where(x => x.CompanyId == companyId && !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title?.Trim()))
            {
                reviewsQuery = reviewsQuery
                    .Where(x => x.Title.ToLower().Contains(title.ToLower().Trim()));
            }

            if (rating != null)
            {
                reviewsQuery = reviewsQuery
                    .Where(x => x.Rating == rating);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (reviewsPerPage < 6)
            {
                reviewsPerPage = 6;
            }
            else if (reviewsPerPage > 96)
            {
                reviewsPerPage = 96;
            }

            var reviews = reviewsQuery
               .OrderByDescending(x => x.CreatedOn)
               .Skip((currentPage - 1) * reviewsPerPage)
               .Take(reviewsPerPage)
               .Select(x => new ReviewListingViewModel
               {
                   Id = x.Id,
                   CandidateFullName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                   CandidateId = x.CandidateId,
                   Title = x.Title,
                   Rating = x.Rating,
                   Content = x.Content,
                   CreatedOn = x.CreatedOn,
                   ModifiedOn = x.ModifiedOn
               })
               .ToList();

            return new ReviewListingQueryModel
            {
                CompanyId = companyId,
                Title = title,
                Rating = rating,
                Reviews = reviews,
                CurrentPage = currentPage,
                ReviewsPerPage = reviewsPerPage,
                TotalReviews = reviewsQuery.Count()
            };
        }

        public ReviewListingQueryModel CandidateReviews(
            string candidateId,
            string companyId,
            string title,
            int? rating,
            int currentPage,
            int reviewsPerPage)
        {
            var reviewsQuery = data
                .Reviews
                .Where(x =>
                x.CandidateId == candidateId
                && !x.IsDeleted)
                .AsQueryable();

            if (companyId != null)
            {
                reviewsQuery = reviewsQuery
                    .Where(x => x.CompanyId == companyId);
            }

            if (!string.IsNullOrEmpty(title?.Trim()))
            {
                reviewsQuery = reviewsQuery
                    .Where(x => x.Title.ToLower().Contains(title.ToLower().Trim()));
            }

            if (rating != null)
            {
                reviewsQuery = reviewsQuery
                    .Where(x => x.Rating == rating);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (reviewsPerPage < 6)
            {
                reviewsPerPage = 6;
            }
            else if (reviewsPerPage > 96)
            {
                reviewsPerPage = 96;
            }

            var reviews = reviewsQuery
                .OrderByDescending(x => x.CreatedOn)
                .Skip((currentPage - 1) * reviewsPerPage)
                .Take(reviewsPerPage)
                .Select(x => new ReviewListingViewModel
                {
                    Id = x.Id,
                    CandidateFullName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                    CandidateId = x.CandidateId,
                    CompanyName = x.Company.Name,
                    CompanyId = x.CompanyId,
                    Title = x.Title,
                    Rating = x.Rating,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    ModifiedOn = x.ModifiedOn
                })
                .ToList();

            return new ReviewListingQueryModel
            {
                CompanyId = companyId,
                CandidateId = candidateId,
                Title = title,
                Rating = rating,
                Reviews = reviews,
                CurrentPage = currentPage,
                ReviewsPerPage = reviewsPerPage,
                TotalReviews = reviewsQuery.Count()
            };
        }
    }
}