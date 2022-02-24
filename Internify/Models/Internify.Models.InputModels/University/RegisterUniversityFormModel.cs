namespace Internify.Models.InputModels.University
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;

    using static Data.Common.DataConstants;

    public class RegisterUniversityFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        [Display(Name = "Website URL")]
        public string WebsiteUrl { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Invalid option.")]
        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }
    }
}