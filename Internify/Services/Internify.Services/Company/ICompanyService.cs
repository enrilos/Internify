namespace Internify.Services.Company
{
    using Models.InputModels.Company;
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

        public bool Edit(
            string id,
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

        public bool AddCandidateToInterns(
            string candidateId,
            string companyId,
            string internshipRole);

        public bool Delete(string id);

        public bool Exists(string id);

        public CompanyDetailsViewModel GetDetailsModel(string id);

        public EditCompanyFormModel GetEditModel(string id);

        public IEnumerable<CompanySelectOptionsViewModel> GetCompaniesSelectOptions();

        public CompanyListingQueryModel All(
            string name,
            string specializationId,
            string countryId,
            int? employeesCount,
            bool isPublic,
            bool isGovernmentOwned,
            int currentPage,
            int companiesPerPage);
    }
}