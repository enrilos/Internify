namespace Internify.Services.Article
{
    using Data;
    using Data.Models;

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
    }
}