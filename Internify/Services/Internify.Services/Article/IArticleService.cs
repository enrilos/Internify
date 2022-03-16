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

        public bool Edit(
            string id,
            string title,
            string content);

        public bool Delete(string id);

        public ArticleDetailsViewModel GetDetailsModel(string id);

        public EditArticleFormModel GetEditModel(string id);

        public bool IsOwnedByCompany(
            string articleId,
            string companyId);

        public ArticleListingQueryModel GetCompanyArticles(
            string companyId,
            string companyName,
            string title,
            int currentPage,
            int articlesPerPage);
    }
}