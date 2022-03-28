namespace Internify.Models.ViewModels.Company
{
    public class CompanyDetailsViewModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string WebsiteUrl { get; init; }

        public int Founded { get; init; }

        public string Description { get; init; }

        public long? RevenueUSD { get; init; }

        public string CEO { get; init; }

        public int EmployeesCount { get; init; }

        public string IsPublicMessage { get; init; }

        public string IsGovernmentOwnedMessage { get; init; }

        public string Specialization { get; init; }

        public string Country { get; init; }
    }
}