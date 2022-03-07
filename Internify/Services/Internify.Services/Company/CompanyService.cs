namespace Internify.Services.Company
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Models.ViewModels.Company;

    using static Common.GlobalConstants;

    public class CompanyService : ICompanyService
    {
        private readonly InternifyDbContext data;
        private readonly UserManager<ApplicationUser> userManager;

        public CompanyService(
            InternifyDbContext data,
            UserManager<ApplicationUser> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

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
            string hostName)
        {
            var company = new Company
            {
                Name = name,
                UserId = userId,
                ImageUrl = imageUrl == null ? Path.Combine(hostName, "/images/company.jpg") : imageUrl.Trim(),
                WebsiteUrl = websiteUrl?.Trim(),
                Founded = founded,
                Description = description.Trim(),
                RevenueUSD = revenueUSD,
                CEO = ceo.Trim(),
                EmployeesCount = employeesCount,
                IsPublic = isPublic,
                IsGovernmentOwned = isGovernmentOwned,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            data.Companies.Add(company);

            Task.Run(async () =>
            {
                var user = await userManager.FindByIdAsync(userId);
                await userManager.AddToRoleAsync(user, CompanyRoleName);
            })
            .GetAwaiter()
            .GetResult();

            data.SaveChanges();

            return company.Id;
        }

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
            string hostName)
        {
            var company = data.Companies.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (company == null)
            {
                return false;
            }

            company.Name = name.Trim();
            company.ImageUrl = imageUrl == null ? Path.Combine(hostName, "/images/company.jpg") : imageUrl.Trim();
            company.WebsiteUrl = websiteUrl?.Trim();
            company.Founded = founded;
            company.Description = description.Trim();
            company.RevenueUSD = revenueUSD;
            company.CEO = ceo.Trim();
            company.EmployeesCount = employeesCount;
            company.IsPublic = isPublic;
            company.IsGovernmentOwned = isGovernmentOwned;
            company.SpecializationId = specializationId;
            company.CountryId = countryId;

            company.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public CompanyDetailsViewModel GetDetailsModel(string id)
            => data
            .Companies
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new CompanyDetailsViewModel
            {
                Id = id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Founded = x.Founded,
                Description = x.Description,
                RevenueUSD = x.RevenueUSD,
                CEO = x.CEO,
                EmployeesCount = x.EmployeesCount,
                IsPublicMessage = x.IsPublic == true ? "✔️ Yes" : "❌ No",
                IsGovernmentOwnedMessage = x.IsGovernmentOwned == true ? "✔️ Yes" : "❌ No",
                Specialization = x.Specialization.Name,
                Country = x.Country.Name,
            })
            .FirstOrDefault();
    }
}