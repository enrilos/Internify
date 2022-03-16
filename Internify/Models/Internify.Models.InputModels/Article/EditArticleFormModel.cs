namespace Internify.Models.InputModels.Article
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Article;

    public class EditArticleFormModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(maximumLength: TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(maximumLength: ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }
    }
}