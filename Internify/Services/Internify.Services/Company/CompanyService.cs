namespace Internify.Services.Company
{
    using Data;

    public class CompanyService : ICompanyService
    {
        private readonly InternifyDbContext data;

        public CompanyService(InternifyDbContext data)
            => this.data = data;

        public bool IsCompany(string userId)
            => data
            .Companies
            .Any(x => x.UserId == userId);

        public string GetIdByUserId(string userId)
            => data
            .Companies
            .Where(x => x.UserId == userId)
            .FirstOrDefault()?.Id;
    }
}
