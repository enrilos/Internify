namespace Internify.Services.Article
{
    using Data;
    using Data.Models;
    using Ganss.XSS;
    using Models.InputModels.Article;
    using Models.ViewModels.Article;

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
            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(content);

            var article = new Article
            {
                Title = title.Trim(),
                ImageUrl = string.IsNullOrEmpty(imageUrl) ? Path.Combine(hostName, "/images/article.jpg") : imageUrl.Trim(),
                Content = sanitizedContent.Trim(),
                CompanyId = companyId
            };

            data.Articles.Add(article);

            data.SaveChanges();

            return article.Id;
        }

        public bool Edit(
            string id,
            string title,
            string content)
        {
            var article = data.Articles.Find(id);

            if (article == null)
            {
                return false;
            }

            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(content);

            article.Title = title.Trim();
            article.Content = sanitizedContent.Trim();
            article.ModifiedOn = DateTime.UtcNow;

            var result = data.SaveChanges();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public bool Delete(string id)
        {
            var article = data.Articles.Find(id);

            if (article == null)
            {
                return false;
            }

            article.IsDeleted = true;
            article.DeletedOn = DateTime.UtcNow;

            var result = data.SaveChanges();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public ArticleDetailsViewModel GetDetailsModel(string id)
            => data
            .Articles
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new ArticleDetailsViewModel
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Content = x.Content,
                CompanyId = x.CompanyId,
                CompanyName = x.Company.Name,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn
            })
            .FirstOrDefault();

        public EditArticleFormModel GetEditModel(string id)
            => data
            .Articles
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new EditArticleFormModel
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content
            })
            .FirstOrDefault();

        public bool IsOwnedByCompany(
            string articleId,
            string companyId)
            => data
            .Articles
            .Any(x =>
            x.Id == articleId
            && x.CompanyId == companyId);

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