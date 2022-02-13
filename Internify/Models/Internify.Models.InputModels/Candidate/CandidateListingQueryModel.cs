namespace Internify.Models.InputModels.Candidate
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Candidate;
    using ViewModels.Country;
    using ViewModels.Specialization;

    public class CandidateListingQueryModel
    {
        // Replace Full name with FirstName and LastName fields?
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Specialization")]
        public string SpecializationId { get; set; }

        public IEnumerable<SpecializationListingViewModel> Specializations { get; set; }

        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public bool IsAvailable { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }

        public IEnumerable<CandidateListingViewModel> Candidates { get; set; }
    }
}