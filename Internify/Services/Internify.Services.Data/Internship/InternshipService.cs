namespace Internify.Services.Data.Internship
{
    using Ganss.XSS;
    using Internify.Data;
    using Internify.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.Internship;
    using Models.ViewModels.Internship;

    public class InternshipService : IInternshipService
    {
        private readonly InternifyDbContext data;

        public InternshipService(InternifyDbContext data)
            => this.data = data;

        public string Add(
            string companyId,
            string role,
            bool isPaid,
            decimal? salaryUSD,
            bool isRemote,
            string description,
            string countryId)
        {
            var sanitizer = new HtmlSanitizer();

            if (!data.Companies.Any(x => x.Id == companyId && !x.IsDeleted)
                || (!isRemote && !data.Countries.Any(x => x.Id == countryId)))
            {
                return null;
            }

            var internship = new Internship
            {
                CompanyId = companyId,
                Role = sanitizer.Sanitize(role).Trim(),
                IsPaid = isPaid,
                SalaryUSD = salaryUSD,
                IsRemote = isRemote,
                Description = sanitizer.Sanitize(description).Trim(),
                CountryId = countryId
            };

            data.Internships.Add(internship);

            data.SaveChanges();

            return internship.Id;
        }

        public bool Edit(
            string id,
            bool isPaid,
            decimal? salaryUSD,
            string description)
        {
            var internship = data.Internships.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (internship == null)
            {
                return false;
            }

            var sanitizer = new HtmlSanitizer();

            internship.IsPaid = isPaid;
            internship.SalaryUSD = salaryUSD;
            internship.Description = sanitizer.Sanitize(description).Trim();

            internship.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var internship = data
                .Internships
                .Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Applications)
                .FirstOrDefault();

            if (internship == null)
            {
                return false;
            }

            internship.IsDeleted = true;
            internship.DeletedOn = DateTime.UtcNow;

            foreach (var application in internship.Applications)
            {
                application.IsDeleted = true;
                application.DeletedOn = internship.DeletedOn;
            }

            data.SaveChanges();

            return true;
        }

        public bool Exists(string id)
            => data
            .Internships
            .Any(x =>
            x.Id == id
            && !x.IsDeleted);

        public bool IsInternshipOwnedByCompany(
            string internshipId,
            string companyId)
            => data
            .Internships
            .Any(x =>
            x.Id == internshipId
            && x.CompanyId == companyId
            && !x.IsDeleted);

        public string GetRoleById(string id)
            => data
            .Internships
            .FirstOrDefault(x =>
            x.Id == id
            && !x.IsDeleted)?.Role;

        public InternshipDetailsViewModel GetDetailsModel(string id)
            => data
            .Internships
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new InternshipDetailsViewModel
            {
                Id = x.Id,
                Role = x.Role,
                IsPaid = x.IsPaid,
                SalaryUSD = x.SalaryUSD,
                IsRemote = x.IsRemote,
                Description = x.Description,
                CompanyImageUrl = x.Company.ImageUrl,
                CompanyId = x.CompanyId,
                CompanyName = x.Company.Name,
                Country = x.Country.Name,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn
            })
            .FirstOrDefault();

        public EditInternshipFormModel GetEditModel(string id)
            => data
            .Internships
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new EditInternshipFormModel
            {
                Id = x.Id,
                IsPaid = x.IsPaid,
                SalaryUSD = x.SalaryUSD,
                Description = x.Description
            })
            .FirstOrDefault();

        public InternshipListingQueryModel All(
            string role,
            bool isPaid,
            bool isRemote,
            string companyId,
            string countryId,
            int currentPage,
            int internshipsPerPage)
        {
            var internshipsQuery = data
                .Internships
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            var sanitizer = new HtmlSanitizer();

            if (!string.IsNullOrEmpty(role))
            {
                var sanitizedRole = sanitizer
                    .Sanitize(role)
                    .Trim();

                internshipsQuery = internshipsQuery
                    .Where(x => x.Role.ToLower().Contains(sanitizedRole.ToLower()));
            }

            // Otherwise, ignore filter
            if (isPaid)
            {
                internshipsQuery = internshipsQuery
                    .Where(x => x.IsPaid);
            }

            // Otherwise, ignore filter
            if (isRemote)
            {
                internshipsQuery = internshipsQuery
                    .Where(x => x.IsRemote);
            }

            if (!string.IsNullOrEmpty(companyId))
            {
                internshipsQuery = internshipsQuery
                    .Where(x => x.CompanyId == companyId);
            }

            if (!string.IsNullOrEmpty(countryId))
            {
                internshipsQuery = internshipsQuery
                    .Where(x => x.CountryId == countryId);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (internshipsPerPage < 6)
            {
                internshipsPerPage = 6;
            }
            else if (internshipsPerPage > 96)
            {
                internshipsPerPage = 96;
            }

            var internships = internshipsQuery
                .OrderByDescending(x => x.CreatedOn)
                .ThenBy(x => x.Role)
                .Skip((currentPage - 1) * internshipsPerPage)
                .Take(internshipsPerPage)
                .Select(x => new InternshipListingViewModel
                {
                    Id = x.Id,
                    Role = x.Role,
                    IsPaid = x.IsPaid,
                    IsRemote = x.IsRemote,
                    CompanyImageUrl = x.Company.ImageUrl,
                    Country = x.Country.Name
                })
               .ToList();

            return new InternshipListingQueryModel
            {
                Role = role,
                IsPaid = isPaid,
                IsRemote = isRemote,
                CompanyId = companyId,
                CountryId = countryId,
                Internships = internships,
                CurrentPage = currentPage,
                InternshipsPerPage = internshipsPerPage,
                TotalInternships = internshipsQuery.Count()
            };
        }
    }
}