namespace Internify.Models.InputModels.Candidate
{
    using Data.Models.Enums;
    using ViewModels.Country;
    using ViewModels.Specialization;
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants;

    public class EditCandidateFormModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(UrlMaxLength)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [MaxLength(UrlMaxLength)]
        [Display(Name = "Website URL")]
        public string WebsiteUrl { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public bool IsAvailable { get; set; }

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