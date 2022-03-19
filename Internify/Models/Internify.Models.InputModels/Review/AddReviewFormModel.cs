namespace Internify.Models.InputModels.Review
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Review;

    public class AddReviewFormModel
    {
        [Required]
        public string CandidateId { get; set; }

        [Required]
        public string CompanyId { get; set; }

        [Required]
        [StringLength(maximumLength: TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [Range(minimum: 1, maximum: 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(maximumLength: ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }
    }
}