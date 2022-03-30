namespace Internify.Services.Data.Company
{
    using Models.InputModels.Company;
    using Models.ViewModels.Company;

    public interface ICompanyService
    {
        bool IsCompany(string id);

        bool IsCompanyByUserId(string userId);

        string GetIdByUserId(string userId);

        string Add(
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

        bool Edit(
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

        bool AddCandidateToInterns(
           string candidateId,
           string companyId,
           string internshipRole);

        bool Delete(string id);

        bool Exists(string id);

        CompanyDetailsViewModel GetDetailsModel(string id);

        EditCompanyFormModel GetEditModel(string id);

        IEnumerable<CompanySelectOptionsViewModel> GetCompaniesSelectOptions();

        CompanyListingQueryModel All(
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