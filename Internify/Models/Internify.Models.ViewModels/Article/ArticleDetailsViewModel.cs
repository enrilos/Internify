namespace Internify.Models.ViewModels.Article
{
    public class ArticleDetailsViewModel
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string ImageUrl { get; init; }

        public string Content { get; init; }

        public string CompanyId { get; init; }

        public string CompanyName { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime? ModifiedOn { get; init; }
    }
}