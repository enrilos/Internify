namespace Internify.Models.InputModels.Article
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants;
    using static Data.Common.DataConstants.Article;

    public class AddArticleFormModel
    {
        [Required]
        public string CompanyId { get; set; }

        [Required]
        [StringLength(maximumLength: TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(maximumLength: ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }
    }
}