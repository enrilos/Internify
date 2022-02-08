namespace Internify.Services.Company
{
    public interface ICompanyService
    {
        public bool IsCompany(string userId);

        public string GetIdByUserId(string userId);
    }
}
