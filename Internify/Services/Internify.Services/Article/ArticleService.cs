namespace Internify.Services.Data.Article
{
    using Ganss.XSS;
    using Internify.Data;
    using Internify.Data.Models;
    using Microsoft.EntityFrameworkCore;
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

            if (!data.Companies.Any(x => x.Id == companyId && !x.IsDeleted))
            {
                return null;
            }

            var article = new Article
            {
                Title = sanitizer.Sanitize(title).Trim(),
                ImageUrl = string.IsNullOrEmpty(imageUrl) ? Path.Combine(hostName, "/images/article.jpg") : sanitizer.Sanitize(imageUrl).Trim(),
                Content = sanitizer.Sanitize(content).Trim(),
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
            var article = data.Articles.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (article == null)
            {
                return false;
            }

            var sanitizer = new HtmlSanitizer();

            article.Title = sanitizer.Sanitize(title).Trim();
            article.Content = sanitizer.Sanitize(content).Trim();
            article.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var article = data
                .Articles
                .Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Comments)
                .FirstOrDefault();

            if (article == null)
            {
                return false;
            }

            article.IsDeleted = true;
            article.DeletedOn = DateTime.UtcNow;

            foreach (var comment in article.Comments)
            {
                comment.IsDeleted = true;
                comment.DeletedOn = article.DeletedOn;
            }

            data.SaveChanges();

            return true;
        }

        public bool Exists(string id)
            => data
            .Articles
            .Any(x => x.Id == id && !x.IsDeleted);

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
            && x.CompanyId == companyId
            && !x.IsDeleted);

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

            var sanitizer = new HtmlSanitizer();

            if (string.IsNullOrEmpty(companyName))
            {
                companyName = data.Companies.FirstOrDefault(x => x.Id == companyId)?.Name;
            }

            if (!string.IsNullOrEmpty(title))
            {
                var sanitizedTitle = sanitizer
                    .Sanitize(title)
                    .Trim();

                articlesQuery = articlesQuery
                    .Where(x => x.Title.ToLower().Contains(sanitizedTitle.ToLower()));
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