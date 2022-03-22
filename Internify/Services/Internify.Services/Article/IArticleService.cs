namespace Internify.Services.Article
{
    using Models.InputModels.Article;
    using Models.ViewModels.Article;

    public interface IArticleService
    {
        string Add(
           string companyId,
           string title,
           string imageUrl,
           string content,
           string hostName);

        bool Edit(
           string id,
           string title,
           string content);

        bool Delete(string id);

        bool Exists(string id);

        ArticleDetailsViewModel GetDetailsModel(string id);

        EditArticleFormModel GetEditModel(string id);

        bool IsOwnedByCompany(
           string articleId,
           string companyId);

        ArticleListingQueryModel GetCompanyArticles(
           string companyId,
           string companyName,
           string title,
           int currentPage,
           int articlesPerPage);
    }
}