namespace Internify.Models.InputModels.University
{
    using Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;

    using static Data.Common.DataConstants;
    using static Data.Common.DataConstants.University;

    public class EditUniversityFormModel
    {
        [Required]
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

        [EnumDataType(typeof(Type))]
        public Type Type { get; set; }

        [Required]
        [StringLength(maximumLength: DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Invalid option.")]
        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }
    }
}