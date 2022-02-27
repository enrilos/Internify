namespace Internify.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.Comment;

    public class Comment
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string ArticleId { get; set; }

        public Article Article { get; set; }
    }
}