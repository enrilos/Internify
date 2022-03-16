namespace Internify.Services.Article
{
    using Models.InputModels.Article;

    public interface IArticleService
    {
        public string Add(
            string companyId,
            string title,
            string imageUrl,
            string content,
            string hostName);

        public ArticleListingQueryModel GetCompanyArticles(
            string companyId,
            string companyName,
            string title,
            int currentPage,
            int articlesPerPage);
    }
}