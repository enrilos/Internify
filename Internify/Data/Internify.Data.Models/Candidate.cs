namespace Internify.Data.Models
{
    using Enums;
    using System.ComponentModel.DataAnnotations;

    using static Internify.Data.Common.DataConstants;
    using static Internify.Data.Common.DataConstants.Candidate;

    public class Candidate : ApplicationUser
    {
        [Key]
        public string CandidateId { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength)]
        public string WebsiteUrl { get; set; }

        [Range(minimum: 18, 100)]
        public int Age { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public string FieldOfStudyId { get; set; }

        public Industry FieldOfStudy { get; set; }

        // Not Required owing to the fact that there could be candidates who have chosen private education (better) over public/government-backed.
        public string UniversityId { get; set; }

        public University University { get; set; }

        [Required]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}