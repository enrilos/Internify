namespace Internify.Models.InputModels.University
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Country;
    using ViewModels.University;

    public class UniversityListingQueryModel
    {
        public string Name { get; set; }

        [Display(Name = "Country")]
        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int UniversitiesPerPage { get; set; }

        public IEnumerable<UniversityListingViewModel> Universities { get; set; }

        public int TotalUniversities { get; set; }
    }
}