namespace Internify.Services.Company
{
    using Data;

    public class CompanyService : ICompanyService
    {
        private readonly InternifyDbContext data;

        public CompanyService(InternifyDbContext data)
            => this.data = data;

        public bool IsCompany(string id)
            => data
            .Companies
            .Any(x => x.Id == id && !x.IsDeleted);

        public bool IsCompanyByUserId(string userId)
            => data
            .Companies
            .Any(x => x.UserId == userId && !x.IsDeleted);

        public string GetIdByUserId(string userId)
            => data
            .Companies
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .FirstOrDefault()?.Id;
    }
}
