namespace Internify.Models.InputModels.Company
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;
    using ViewModels.Specialization;

    using static Data.Common.DataConstants;
    using static Data.Common.DataConstants.Company;

    public class RegisterCompanyFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength)]
        [Display(Name = "Website URL")]
        public string WebsiteUrl { get; set; }

        [Range(minimum: FoundedMinYear, maximum: FoundedMaxYear)]
        public int Founded { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Range(typeof(decimal), "0", "999999999999999")]
        public decimal Revenue { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string CEO { get; set; }

        [Range(minimum: 0, maximum: 999999999)]
        public int EmployeesCount { get; set; }

        public bool IsPublic { get; set; }

        public bool IsGovernmentOwned { get; set; }

        [Required(ErrorMessage = "Invalid option.")]
        [Display(Name = "Specialization")]
        public string SpecializationId { get; set; }

        public IEnumerable<SpecializationListingViewModel> Specializations { get; set; }

        [Required(ErrorMessage = "Invalid option.")]
        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }
    }
}