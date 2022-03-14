namespace Internify.Models.InputModels.Internship
{
    using ViewModels.Internship;
    using System.ComponentModel.DataAnnotations;

    public class InternshipApplicantListingQueryModel
    {
        public string InternshipId { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int ApplicantsPerPage { get; set; }

        public IEnumerable<InternshipApplicantListingViewModel> Applicants { get; set; }

        public int TotalApplicants { get; set; }
    }
}