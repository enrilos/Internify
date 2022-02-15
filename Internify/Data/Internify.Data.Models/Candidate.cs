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

        // Not Required owing to the fact that there could be candidates who have chosen private education (better) over public/government-backed.
        // Candidate will have to apply to the univesities alumni which is decided by each university.
        // After each university realizes that a/the candidate is one of their graduate, they will accept the individual in their alumni list.
        public string UniversityId { get; set; }

        public University University { get; set; }

        [Required]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}