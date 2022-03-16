namespace Internify.Models.InputModels.Application
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Application;

    public class ApplicationFormModel
    {
        public string Id { get; set; }

        [Required]
        public string InternshipId { get; set; }

        [Required]
        public string CandidateId { get; set; }

        [Required]
        [StringLength(maximumLength: CoverLetterMaxLength, MinimumLength = CoverLetterMinLength)]
        [Display(Name = "Cover Letter")]
        public string CoverLetter { get; set; }
    }
}