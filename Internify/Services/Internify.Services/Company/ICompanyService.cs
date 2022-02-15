namespace Internify.Services.Company
{
    public interface ICompanyService
    {
        public bool IsCompanyByUserId(string userId);

        public string GetIdByUserId(string userId);
    }
}
