namespace Internify.Services.Company
{
    using Data;
    using Data.Models;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
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
            var sanitizer = new HtmlSanitizer();

            if (!data.Users.Any(x => x.Id == userId && !x.IsDeleted)
                || !data.Specializations.Any(x => x.Id == specializationId)
                || !data.Countries.Any(x => x.Id == countryId))
            {
                return null;
            }

            var company = new Company
            {
                Name = sanitizer.Sanitize(name).Trim(),
                UserId = userId,
                ImageUrl = string.IsNullOrEmpty(imageUrl) ? Path.Combine(hostName, "/images/company.jpg") : sanitizer.Sanitize(imageUrl).Trim(),
                WebsiteUrl = sanitizer.Sanitize(websiteUrl)?.Trim(),
                Founded = founded,
                Description = sanitizer.Sanitize(description).Trim(),
                RevenueUSD = revenueUSD,
                CEO = sanitizer.Sanitize(ceo).Trim(),
                EmployeesCount = employeesCount,
                IsPublic = isPublic,
                IsGovernmentOwned = isGovernmentOwned,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            data.Companies.Add(company);

            // Used for testing purposes.
            if (userManager != null)
            {
                Task.Run(async () =>
                {
                    var user = await userManager.FindByIdAsync(userId);
                    await userManager.AddToRoleAsync(user, CompanyRoleName);
                })
                .GetAwaiter()
                .GetResult();
            }

            var result = data.SaveChanges();

            if (result == 0)
            {
                return null;
            }

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

            if (company == null
                || !data.Specializations.Any(x => x.Id == specializationId)
                || !data.Countries.Any(x => x.Id == countryId))
            {
                return false;
            }

            var sanitizer = new HtmlSanitizer();

            company.Name = sanitizer.Sanitize(name).Trim();
            company.ImageUrl = string.IsNullOrEmpty(imageUrl) ? Path.Combine(hostName, "/images/company.jpg") : sanitizer.Sanitize(imageUrl).Trim();
            company.WebsiteUrl = sanitizer.Sanitize(websiteUrl)?.Trim();
            company.Founded = founded;
            company.Description = sanitizer.Sanitize(description).Trim();
            company.RevenueUSD = revenueUSD;
            company.CEO = sanitizer.Sanitize(ceo).Trim();
            company.EmployeesCount = employeesCount;
            company.IsPublic = isPublic;
            company.IsGovernmentOwned = isGovernmentOwned;
            company.SpecializationId = specializationId;
            company.CountryId = countryId;

            company.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public bool AddCandidateToInterns(
            string candidateId,
            string companyId,
            string internshipRole)
        {
            var candidate = data
                .Candidates
                .Where(x => x.Id == candidateId && !x.IsDeleted)
                .Include(x => x.Applications)
                .FirstOrDefault();

            var companyDataId = data
                .Companies
                .FirstOrDefault(x => x.Id == companyId && !x.IsDeleted)?.Id;

            if (candidate == null
                || companyDataId == null)
            {
                return false;
            }

            candidate.CompanyId = companyDataId;
            candidate.InternshipRole = internshipRole;

            var deleteDate = DateTime.UtcNow;

            foreach (var application in candidate.Applications)
            {
                application.IsDeleted = true;
                application.DeletedOn = deleteDate;
            }

            data.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var company = data
                   .Companies
                   .Where(x => x.Id == id && !x.IsDeleted)
                   .Include(x => x.OpenInternships)
                   .Include(x => x.Interns)
                   .Include(x => x.Articles)
                   .Include(x => x.Reviews)
                   .FirstOrDefault();

            if (company == null)
            {
                return false;
            }

            company.IsDeleted = true;
            company.DeletedOn = DateTime.UtcNow;

            foreach (var internship in company.OpenInternships)
            {
                internship.IsDeleted = true;
                internship.DeletedOn = company.DeletedOn;
            }

            foreach (var intern in company.Interns)
            {
                intern.CompanyId = null;
            }

            foreach (var article in company.Articles)
            {
                article.IsDeleted = true;
                article.DeletedOn = company.DeletedOn;
            }

            foreach (var review in company.Reviews)
            {
                review.IsDeleted = true;
                review.DeletedOn = company.DeletedOn;
            }

            data.SaveChanges();

            if (userManager != null)
            {
                Task.Run(async () =>
                {
                    var user = await userManager.FindByIdAsync(company.UserId);
                    user.IsDeleted = company.IsDeleted;
                    user.DeletedOn = company.DeletedOn;
                })
                .GetAwaiter()
                .GetResult();
            }

            data.SaveChanges();

            return true;
        }

        public bool Exists(string id)
            => data
            .Companies
            .Any(x => x.Id == id && !x.IsDeleted);

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

        public IEnumerable<CompanySelectOptionsViewModel> GetCompaniesSelectOptions()
            => data
            .Companies
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => new CompanySelectOptionsViewModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();

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

            var sanitizer = new HtmlSanitizer();

            if (!string.IsNullOrEmpty(name))
            {
                var sanitizedName = sanitizer
                    .Sanitize(name)
                    .Trim();

                companiesQuery = companiesQuery
                    .Where(x => x.Name.ToLower().Contains(sanitizedName.ToLower()));
            }

            if (!string.IsNullOrEmpty(specializationId))
            {
                companiesQuery = companiesQuery
                    .Where(x => x.SpecializationId == specializationId);
            }

            if (!string.IsNullOrEmpty(countryId))
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

            // Otherwise, list all.
            if (isPublic)
            {
                companiesQuery = companiesQuery
                    .Where(x => x.IsPublic);
            }

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