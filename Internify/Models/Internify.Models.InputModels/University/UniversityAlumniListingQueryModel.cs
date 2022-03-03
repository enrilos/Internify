namespace Internify.Models.InputModels.University
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Candidate;

    public class UniversityAlumniListingQueryModel
    {
        public string Id { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int AlumniPerPage { get; set; }

        public IEnumerable<CandidateListingViewModel> Alumni { get; set; }

        public int TotalAlumni { get; set; }
    }
}