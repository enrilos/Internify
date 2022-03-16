namespace Internify.Services.Article
{
    using Data;
    using Data.Models;
    using Internify.Models.ViewModels.Article;
    using Models.InputModels.Article;

    public class ArticleService : IArticleService
    {
        private readonly InternifyDbContext data;

        public ArticleService(InternifyDbContext data)
            => this.data = data;

        public string Add(
            string companyId,
            string title,
            string imageUrl,
            string content,
            string hostName)
        {
            var article = new Article
            {
                Title = title.Trim(),
                ImageUrl = string.IsNullOrEmpty(imageUrl) ? Path.Combine(hostName, "/images/article.jpg") : imageUrl.Trim(),
                Content = content.Trim(),
                CompanyId = companyId
            };

            data.Articles.Add(article);

            data.SaveChanges();

            return article.Id;
        }

        public ArticleListingQueryModel GetCompanyArticles(
            string companyId,
            string companyName,
            string title,
            int currentPage,
            int articlesPerPage)
        {
            var articlesQuery = data
                .Articles
                .Where(x =>
                x.CompanyId == companyId
                && !x.IsDeleted)
                .AsQueryable();

            if (string.IsNullOrEmpty(companyName?.Trim()))
            {
                companyName = data.Companies.FirstOrDefault(x => x.Id == companyId)?.Name;
            }

            if (title != null)
            {
                articlesQuery = articlesQuery
                    .Where(x => x.Title.ToLower().Contains(title.ToLower().Trim()));
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (articlesPerPage < 6)
            {
                articlesPerPage = 6;
            }
            else if (articlesPerPage > 96)
            {
                articlesPerPage = 96;
            }

            var articles = articlesQuery
               .OrderByDescending(x => x.CreatedOn)
               .ThenBy(x => x.Title)
               .Skip((currentPage - 1) * articlesPerPage)
               .Take(articlesPerPage)
               .Select(x => new ArticleListingViewModel
               {
                   Id = x.Id,
                   Title = x.Title,
                   ImageUrl = x.ImageUrl
               })
               .ToList();

            return new ArticleListingQueryModel
            {
                CompanyId = companyId,
                CompanyName = companyName,
                Title = title,
                Articles = articles,
                CurrentPage = currentPage,
                ArticlesPerPage = articlesPerPage,
                TotalArticles = articlesQuery.Count()
            };
        }
    }
}