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

        public decimal Revenue { get; init; }

        public string CEO { get; init; }

        public int EmployeesCount { get; init; }

        public string IsPublicMessage { get; init; }

        public string IsGovernmentOwnedMessage { get; init; }

        public string Specialization { get; init; }

        public string Country { get; init; }

        // Additional button in details that redirect to a separate action Company/OpenInternships?companyId=X
        //public ICollection<Internship> OpenInternships { get; init; } = new List<Internship>();

        // Additional button in details that redirect to a separate action Company/Interns?companyId=X
        //public ICollection<Candidate> Interns { get; init; } = new List<Candidate>();

        // Additional button in details that redirect to a separate action Company/Articles?companyId=X
        //public ICollection<Article> Articles { get; init; } = new List<Article>();

        // Additional button in details that redirect to a separate action Company/Reviews?companyId=X
        //public ICollection<Review> Reviews { get; init; } = new List<Review>();
    }
}