namespace Internify.Data.Models
{
    using Common;
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.Application;

    // i.e. candidates apply for a particular internship
    public class Application : IAuditInfo, IDeletableEntity
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string InternshipId { get; set; }

        public Internship Internship { get; set; }

        [Required]
        [MaxLength(CoverLetterMaxLength)]
        public string CoverLetter { get; set; }

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}