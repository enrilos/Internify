namespace Internify.Models.InputModels.Review
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Company;
    using ViewModels.Review;

    public class ReviewListingQueryModel
    {
        public string CandidateId { get; set; }

        public string Title { get; set; }

        [Range(minimum: 1, maximum: 5)]
        public int? Rating { get; set; }

        [Display(Name = "Company")]
        public string CompanyId { get; set; }

        public IEnumerable<CompanySelectOptionsViewModel> Companies { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int ReviewsPerPage { get; set; }

        public IEnumerable<ReviewListingViewModel> Reviews { get; set; }

        public int TotalReviews { get; set; }
    }
}