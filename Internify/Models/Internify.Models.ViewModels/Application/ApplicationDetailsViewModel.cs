namespace Internify.Models.ViewModels.Application
{
    public class ApplicationDetailsViewModel
    {
        public string Id { get; init; }

        public string Role { get; init; }
        
        public string InternshipId { get; init; }

        public string Company { get; init; }

        public string CompanyId { get; init; }

        public string CoverLetter { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime? ModifiedOn { get; set; }
    }
}