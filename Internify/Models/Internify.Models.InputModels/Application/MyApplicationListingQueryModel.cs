namespace Internify.Models.InputModels.Application
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Application;
    using ViewModels.Company;

    public class MyApplicationListingQueryModel
    {
        public string CandidateId { get; set; }

        public string Role { get; set; }

        [Display(Name = "Company")]
        public string CompanyId { get; set; }

        public IEnumerable<CompanySelectOptionsViewModel> Companies { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int ApplicationsPerPage { get; set; }

        public IEnumerable<MyApplicationListingViewModel> Applications { get; set; }

        public int TotalApplications { get; set; }
    }
}