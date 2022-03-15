namespace Internify.Models.InputModels.Internship
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;
    using ViewModels.Internship;
    using ViewModels.Specialization;

    public class InternshipApplicantListingQueryModel
    {
        [Display(Name = "First Name")]
        public string ApplicantFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string ApplicantLastName { get; set; }

        [Display(Name = "Specialization")]
        public string ApplicantSpecializationId { get; set; }

        public IEnumerable<SpecializationListingViewModel> Specializations { get; set; }

        [Display(Name = "Country")]
        public string ApplicantCountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }

        public string InternshipId { get; set; }

        public string InternshipRole { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int ApplicantsPerPage { get; set; }

        public IEnumerable<InternshipApplicantListingViewModel> Applicants { get; set; }

        public int TotalApplicants { get; set; }
    }
}