namespace Internify.Models.InputModels.Internship
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;

    using static Data.Common.DataConstants;
    using static Data.Common.DataConstants.Internship;

    public class AddInternshipFormModel
    {
        [Required]
        [StringLength(maximumLength: RoleMaxLength, MinimumLength = RoleMinLength)]
        public string Role { get; set; }

        public bool IsPaid { get; set; }

        [Range(typeof(decimal), "1", "9999999")]
        public decimal? SalaryUSD { get; set; }

        public bool IsRemote { get; set; }

        [Required]
        [StringLength(maximumLength: DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        public string CompanyId { get; set; }

        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }
    }
}