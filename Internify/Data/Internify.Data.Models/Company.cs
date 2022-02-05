namespace Internify.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants;
    using static Common.DataConstants.Company;

    public class Company : ApplicationUser
    {
        [Key]
        public string CompanyId { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength)]
        public string WebsiteUrl { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public decimal Revenue { get; set; }

        [Required]
        [MaxLength(CEOMaxLength)]
        public string CEO { get; set; }

        public int EmployeesCount { get; set; }

        // E.g. People can own part of the company (shares)
        public bool IsPublic { get; set; }

        public bool IsGovernmentOwned { get; set; }

        [Required]
        public string IndustryId { get; set; }

        public Industry Industry { get; set; }

        [Required]
        public string CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<Internship> OpenInternships { get; set; } = new List<Internship>();

        public ICollection<Candidate> Interns { get; set; } = new List<Candidate>();

        public ICollection<Candidate> FormerInterns { get; set; } = new List<Candidate>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}