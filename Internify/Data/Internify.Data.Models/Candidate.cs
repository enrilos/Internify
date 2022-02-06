namespace Internify.Data.Models
{
    using Enums;
    using System.ComponentModel.DataAnnotations;

    using static Internify.Data.Common.DataConstants;

    public class Candidate
    {
        [Key]
        public string CandidateId { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength)]
        public string WebsiteUrl { get; set; }

        [Range(minimum: 18, maximum: 100)]
        public int Age { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public string SpecializationId { get; set; }

        public Specialization Specialization { get; set; }

        // Not Required owing to the fact that there could be candidates who have chosen private education (better) over public/government-backed.
        public string UniversityId { get; set; }

        public University University { get; set; }

        [Required]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        // TODO: Only former interns should be allowed to write reviews
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}