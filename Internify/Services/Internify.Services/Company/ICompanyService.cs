namespace Internify.Services.Company
{
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
            decimal revenue,
            string ceo,
            int employeesCount,
            bool isPublic,
            bool isGovernmentOwned,
            string specializationId,
            string countryId,
            string hostName);
    }
}