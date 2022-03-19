﻿namespace Internify.Services.Comment
{
    using Data;
    using Data.Models;
    using Ganss.XSS;
    using Internify.Models.InputModels.Comment;
    using Internify.Models.ViewModels.Comment;

    public class CommentService : ICommentService
    {
        private readonly InternifyDbContext data;

        public CommentService(InternifyDbContext data)
            => this.data = data;

        public string CommentArticle(
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

            data.SaveChanges();

            return comment.Id;
        }

        public CommentListingQueryModel ArticleComments(
           string articleId,
           int currentPage,
           int commentsPerPage)
        {
            var commentsQuery = data
                .Comments
                .Where(x => x.ArticleId == articleId && !x.IsDeleted)
                .AsQueryable();

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (commentsPerPage < 6)
            {
                commentsPerPage = 6;
            }
            else if (commentsPerPage > 96)
            {
                commentsPerPage = 96;
            }

            var comments = commentsQuery
                .OrderByDescending(x => x.CreatedOn)
                .Skip((currentPage - 1) * commentsPerPage)
                .Take(commentsPerPage)
                .Select(x => new CommentListingViewModel
                {
                    CandidateFullName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                    CandidateId = x.CandidateId,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn
                })
                .ToList();

            return new CommentListingQueryModel
            {
                ArticleId = articleId,
                CurrentPage = currentPage,
                CommentsPerPage = commentsPerPage,
                Comments = comments,
                TotalComments = commentsQuery.Count()
            };
        }
    }
}