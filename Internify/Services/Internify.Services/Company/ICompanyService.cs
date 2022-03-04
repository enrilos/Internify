namespace Internify.Services.Company
{
    using Models.ViewModels.Company;

    public interface ICompanyService
    {
        public bool IsCompany(string id);

        public bool IsCompanyByUserId(string userId);

        public string GetIdByUserId(string userId);

        public string Add(
            string userId,
            string name,
            string imageUrl,
            string websiteUrl,
            int founded,
            string description,
            long? revenueUSD,
            string ceo,
            int employeesCount,
            bool isPublic,
            bool isGovernmentOwned,
            string specializationId,
            string countryId,
            string hostName);

        public CompanyDetailsViewModel GetDetailsModel(string id);
    }
}