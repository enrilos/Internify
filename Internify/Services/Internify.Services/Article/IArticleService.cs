namespace Internify.Services.Article
{
    using Models.InputModels.Article;
    using Models.ViewModels.Article;

    public interface IArticleService
    {
        public string Add(
            string companyId,
            string title,
            string imageUrl,
            string content,
            string hostName);

        public ArticleDetailsViewModel GetDetailsModel(string id);

        public ArticleListingQueryModel GetCompanyArticles(
            string companyId,
            string companyName,
            string title,
            int currentPage,
            int articlesPerPage);
    }
}