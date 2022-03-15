namespace Internify.Models.InputModels.Intern
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Intern;

    public class InternListingQueryModel
    {
        public string CompanyId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role")]
        public string InternshipRole { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Per Page")]
        public int InternsPerPage { get; set; }

        public IEnumerable<InternListingViewModel> Interns { get; set; }

        public int TotalInterns { get; set; }
    }
}