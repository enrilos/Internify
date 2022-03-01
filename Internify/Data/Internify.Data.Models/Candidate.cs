namespace Internify.Data.Models
{
    using Enums;
    using Common;
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants;

    public class Candidate : IAuditInfo, IDeletableEntity
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string UserId { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength)]
        public string WebsiteUrl { get; set; }

        public DateTime BirthDate { get; set; }


        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required]
        public string SpecializationId { get; set; }

        public Specialization Specialization { get; set; }

        [Required]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<CandidateUniversity> Universities { get; set; } = new List<CandidateUniversity>();

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}