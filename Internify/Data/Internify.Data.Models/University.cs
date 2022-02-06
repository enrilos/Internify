namespace Internify.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants;

    public class University
    {
        [Key]
        public string UniversityId { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string WebsiteUrl { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<Candidate> Alumni { get; set; } = new List<Candidate>();
    }
}