namespace Internify.Models.InputModels.Article
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Article;

    using static Data.Common.DataConstants.Article;

    public class ArticleListingQueryModel
    {
        public string CompanyId { get; set; }

        public string CompanyName { get; set; }

        [StringLength(maximumLength: TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int ArticlesPerPage { get; set; }

        public IEnumerable<ArticleListingViewModel> Articles { get; set; }

        public int TotalArticles { get; set; }
    }
}