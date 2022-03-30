namespace Internify.Services.Data.Comment
{
    using Internify.Models.InputModels.Comment;

    public interface ICommentService
    {
        string CommentArticle(
            string articleId,
            string candidateId,
            string content);

        CommentListingQueryModel ArticleComments(
            string articleId,
            int currentPage,
            int commentsPerPage);
    }
}