namespace Internify.Models.InputModels.University
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;

    using static Data.Common.DataConstants;
    using static Data.Common.DataConstants.University;

    public class EditUniversityFormModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string WebsiteUrl { get; set; }

        [Range(minimum: FoundedMinYear, maximum: FoundedMaxYear)]
        public int Founded { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Invalid option.")]
        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }
    }
}