namespace Internify.Models.InputModels.Application
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Application;

    public class AddApplicationFormModel
    {
        [Required]
        public string InternshipId { get; set; }

        [Required]
        public string CandidateId { get; set; }

        [Required]
        [MaxLength(CoverLetterMaxLength)]
        [Display(Name = "Cover Letter")]
        public string CoverLetter { get; set; }
    }
}