namespace Internify.Models.ViewModels.Internship
{
    public class InternshipDetailsViewModel
    {
        public string Id { get; init; }

        public string Role { get; init; }

        public bool IsPaid { get; init; }

        public decimal? SalaryUSD { get; init; }

        public bool IsRemote { get; init; }

        public string Description { get; init; }

        public string CompanyImageUrl { get; init; }

        public string CompanyId { get; init; }
        
        public string CompanyName { get; init; }

        public string Country { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime? ModifiedOn { get; init; }
    }
}