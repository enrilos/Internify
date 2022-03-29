namespace Internify.Web.Areas.Candidate.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Comment;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Article;
    using Services.Data.Candidate;
    using Services.Data.Comment;

    using static Common.WebConstants;

    public class CommentController : CandidateControllerBase
    {
        private readonly ICommentService commentService;
        private readonly IArticleService articleService;
        private readonly ICandidateService candidateService;

        public CommentController(
            ICommentService commentService,
            IArticleService articleService,
            ICandidateService candidateService)
        {
            this.commentService = commentService;
            this.articleService = articleService;
            this.candidateService = candidateService;
        }

        public IActionResult CommentArticle(string articleId)
        {
            if (!articleService.Exists(articleId))
            {
                return NotFound();
            }

            var candidateId = candidateService.GetIdByUserId(User.Id());

            var commentModel = new AddCommentFormModel
            {
                ArticleId = articleId,
                CandidateId = candidateId
            };

            return View(commentModel);
        }

        [HttpPost]
        public IActionResult CommentArticle(AddCommentFormModel comment)
        {
            if (!articleService.Exists(comment.ArticleId)
                || !candidateService.Exists(comment.CandidateId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            var id = commentService.CommentArticle(
                comment.ArticleId,
                comment.CandidateId,
                comment.Content);

            if (id == null)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = "Successfully posted comment.";

            return RedirectToAction("ArticleComments", "Comment", new { articleId = comment.ArticleId });
        }
    }
}