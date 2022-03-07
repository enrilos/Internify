namespace Internify.Services.Company
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Models.InputModels.Company;
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

        public EditCompanyFormModel GetEditModel(string id)
            => data
            .Companies
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new EditCompanyFormModel
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Founded = x.Founded,
                Description = x.Description,
                RevenueUSD = x.RevenueUSD,
                CEO = x.CEO,
                EmployeesCount = x.EmployeesCount,
                IsPublic = x.IsPublic,
                IsGovernmentOwned = x.IsGovernmentOwned,
                SpecializationId = x.SpecializationId,
                CountryId = x.CountryId
            })
            .FirstOrDefault();

        public CompanyListingQueryModel All(
           string name,
           string specializationId,
           string countryId,
           int? employeesCount,
           bool isPublic,
           bool isGovernmentOwned,
           int currentPage,
           int companiesPerPage)
        {
            var companiesQuery = data
                .Companies
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (name != null)
            {
                companiesQuery = companiesQuery
                    .Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));
            }

            if (specializationId != null)
            {
                companiesQuery = companiesQuery
                    .Where(x => x.SpecializationId == specializationId);
            }

            if (countryId != null)
            {
                companiesQuery = companiesQuery
                    .Where(x => x.CountryId == countryId);
            }

            if (employeesCount != null)
            {
                if (employeesCount <= 50)
                {
                    companiesQuery = companiesQuery
                        .Where(x => x.EmployeesCount <= 50);
                }
                else if (employeesCount > 50)
                {
                    companiesQuery = companiesQuery
                        .Where(x => x.EmployeesCount > 50);
                }
            }

            if (isPublic)
            {
                companiesQuery = companiesQuery
                    .Where(x => x.IsPublic);
            }
            // Otherwise, list all.

            if (isGovernmentOwned)
            {
                companiesQuery = companiesQuery
                    .Where(x => x.IsGovernmentOwned);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (companiesPerPage < 6)
            {
                companiesPerPage = 6;
            }
            else if (companiesPerPage > 96)
            {
                companiesPerPage = 96;
            }

            var companies = companiesQuery
               .OrderByDescending(x => x.CreatedOn)
               .ThenBy(x => x.Name)
               .Skip((currentPage - 1) * companiesPerPage)
               .Take(companiesPerPage)
               .Select(x => new CompanyListingViewModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   ImageUrl = x.ImageUrl,
                   Specialization = x.Specialization.Name,
                   Country = x.Country.Name
               })
               .ToList();

            return new CompanyListingQueryModel
            {
                Name = name,
                SpecializationId = specializationId,
                CountryId = countryId,
                EmployeesCount = employeesCount,
                IsPublic = isPublic,
                IsGovernmentOwned = isGovernmentOwned,
                Companies = companies,
                CurrentPage = currentPage,
                CompaniesPerPage = companiesPerPage,
                TotalCompanies = companiesQuery.Count()
            };
        }
    }
}