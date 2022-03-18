namespace Internify.Services.Comment
{
    public interface ICommentService
    {
        bool CommentArticle(
            string articleId,
            string candidateId,
            string content);
    }
}