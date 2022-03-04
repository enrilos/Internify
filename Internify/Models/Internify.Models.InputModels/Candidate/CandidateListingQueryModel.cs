namespace Internify.Models.InputModels.Candidate
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Candidate;
    using ViewModels.Country;
    using ViewModels.Specialization;

    public class CandidateListingQueryModel
    {
        // Used for university id so that their alumni could be listed. Everything else is identical.
        public string UniversityId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Specialization")]
        public string SpecializationId { get; set; }

        public IEnumerable<SpecializationListingViewModel> Specializations { get; set; }

        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }

        public bool IsAvailable { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int CandidatesPerPage { get; set; }

        public IEnumerable<CandidateListingViewModel> Candidates { get; set; }

        public int TotalCandidates { get; set; }
    }
}