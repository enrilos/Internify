namespace Internify.Services.Comment
{
    using Data;
    using Data.Models;
    using Ganss.XSS;

    public class CommentService : ICommentService
    {
        private readonly InternifyDbContext data;

        public CommentService(InternifyDbContext data)
            => this.data = data;

        public bool CommentArticle(
            string articleId,
            string candidateId,
            string content)
        {
            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(content);

            var comment = new Comment
            {
                ArticleId = articleId,
                CandidateId = candidateId,
                Content = sanitizedContent.Trim(),
            };

            data.Comments.Add(comment);

            var result = data.SaveChanges();

            if (result == 0)
            {
                return false;
            }

            return true;
        }
    }
}