namespace Internify.Models.InputModels.Comment
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Comment;

    public class AddCommentFormModel
    {
        [Required]
        public string ArticleId { get; set; }

        [Required]
        public string CandidateId { get; set; }

        [Required]
        [StringLength(maximumLength: ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }
    }
}