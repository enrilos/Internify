namespace Internify.Data.Models
{
    using Common;
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants;
    using static Common.DataConstants.Internship;

    public class Internship : IAuditInfo, IDeletableEntity
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(RoleMaxLength)]
        public string Role { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public bool IsPaid { get; set; }

        [Range(typeof(decimal), "0", "9999999")]
        public decimal? Salary { get; set; }

        public bool IsRemote { get; set; }

        [Required]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        // Not Required since it could be remote.
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}