namespace Internify.Data.Models
{
    using Common;
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.Review;

    public class Review : IAuditInfo, IDeletableEntity
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        [Required]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}