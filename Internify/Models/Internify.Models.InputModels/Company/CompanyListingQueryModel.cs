namespace Internify.Models.InputModels.Company
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Company;
    using ViewModels.Country;
    using ViewModels.Specialization;

    public class CompanyListingQueryModel
    {
        public string Name { get; set; }

        [Display(Name = "Specialization")]
        public string SpecializationId { get; set; }

        public IEnumerable<SpecializationListingViewModel> Specializations { get; set; }

        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }

        [Display(Name = "Company size")]
        public int? EmployeesCount { get; set; }

        public bool IsPublic { get; set; }

        public bool IsGovernmentOwned { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int CompaniesPerPage { get; set; }

        public IEnumerable<CompanyListingViewModel> Companies { get; set; }

        public int TotalCompanies { get; set; }
    }
}