namespace Internify.Data.Models
{
    using Common;
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants;
    using static Common.DataConstants.Company;

    public class Company : IAuditInfo, IDeletableEntity
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength)]
        public string WebsiteUrl { get; set; }

        [Range(minimum: FoundedMinYear, maximum: FoundedMaxYear)]
        public int Founded { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Range(typeof(decimal), "0", "999999999999999")]
        public long? RevenueUSD { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string CEO { get; set; }

        [Range(minimum: 0, maximum: 999999999)]
        public int EmployeesCount { get; set; }

        // E.g. People can own part of the company (shares)
        public bool IsPublic { get; set; }

        public bool IsGovernmentOwned { get; set; }

        [Required]
        public string SpecializationId { get; set; }

        public Specialization Specialization { get; set; }

        [Required]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<Internship> OpenInternships { get; set; } = new List<Internship>();

        public ICollection<Candidate> Interns { get; set; } = new List<Candidate>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}