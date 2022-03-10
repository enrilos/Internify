namespace Internify.Models.InputModels.Internship
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Company;
    using ViewModels.Country;
    using ViewModels.Internship;

    public class InternshipListingQueryModel
    {
        public string Role { get; set; }

        public bool IsPaid { get; set; }

        public bool IsRemote { get; set; }

        [Display(Name = "Company")]
        public string CompanyId { get; set; }

        public IEnumerable<CompanySelectOptionsViewModel> Companies { get; set; }

        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int InternshipsPerPage { get; set; }

        public IEnumerable<InternshipListingViewModel> Internships { get; set; }

        public int TotalInternships { get; set; }
    }
}