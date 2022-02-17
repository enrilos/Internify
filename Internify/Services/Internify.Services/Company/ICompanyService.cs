namespace Internify.Services.Company
{
    public interface ICompanyService
    {
        public bool IsCompany(string id);

        public bool IsCompanyByUserId(string userId);

        public string GetIdByUserId(string userId);
    }
}
