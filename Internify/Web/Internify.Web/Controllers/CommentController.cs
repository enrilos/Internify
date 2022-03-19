namespace Internify.Web.Controllers
{
    using Internify.Models.InputModels.Comment;
    using Microsoft.AspNetCore.Mvc;
    using Services.Article;
    using Services.Comment;

    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IArticleService articleService;

        public CommentController(
            ICommentService commentService,
            IArticleService articleService)
        {
            this.commentService = commentService;
            this.articleService = articleService;
        }

        public IActionResult ArticleComments([FromQuery] CommentListingQueryModel queryModel)
        {
            if (!articleService.Exists(queryModel.ArticleId))
            {
                return NotFound();
            }

            queryModel = commentService.ArticleComments(
                queryModel.ArticleId,
                queryModel.CurrentPage,
                queryModel.CommentsPerPage);

            return View(queryModel);
        }
    }
}