namespace Internify.Models.ViewModels.Internship
{
    public class InternshipListingViewModel
    {
        public string Id { get; init; }

        public string Role { get; init; }

        public bool IsPaid { get; init; }

        public bool IsRemote { get; init; }

        public string CompanyImageUrl { get; init; }

        public string Country { get; init; }
    }
}