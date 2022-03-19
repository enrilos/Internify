namespace Internify.Models.InputModels.Comment
{
    using Models.ViewModels.Comment;
    using System.ComponentModel.DataAnnotations;

    public class CommentListingQueryModel
    {
        public string ArticleId { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int CommentsPerPage { get; set; }

        public IEnumerable<CommentListingViewModel> Comments { get; set; }

        public int TotalComments { get; set; }
    }
}