namespace Internify.Models.InputModels.Candidate
{
    using Data.Common.Attributes;
    using Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;
    using ViewModels.Specialization;

    using static Data.Common.DataConstants;

    // TODO: Separate Error messages.
    public class BecomeCandidateFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = $"{nameof(FirstName)} must be between 2 and 70 characters long.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = $"{nameof(LastName)} must be between 2 and 70 characters long.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(DescriptionMaxLength, ErrorMessage = $"{nameof(Description)} must be less than 4000 characters long.")]
        public string Description { get; set; }

        [MaxLength(UrlMaxLength, ErrorMessage = $"{nameof(ImageUrl)} must be less than 500 characters long.")]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength, ErrorMessage = $"{nameof(WebsiteUrl)} must be less than 500 characters long.")]
        [Display(Name = "Website URL")]
        public string WebsiteUrl { get; set; }

        [BirthDate(ErrorMessage = $"{nameof(BirthDate)} is invalid.")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = $"{nameof(Gender)} is invalid.")]
        public Gender Gender { get; set; }

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