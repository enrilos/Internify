namespace Internify.Services.Article
{
    public interface IArticleService
    {
        public string Add(
            string companyId,
            string title,
            string imageUrl,
            string content,
            string hostName);
    }
}