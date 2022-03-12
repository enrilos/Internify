namespace Internify.Services.Internship
{
    using Data;
    using Data.Models;
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
            var internship = new Internship
            {
                CompanyId = companyId,
                Role = role,
                IsPaid = isPaid,
                SalaryUSD = salaryUSD,
                IsRemote = isRemote,
                Description = description,
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

            internship.IsPaid = isPaid;
            internship.SalaryUSD = salaryUSD;
            internship.Description = description;

            internship.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var internship = data
                .Internships
                .Include(x => x.Applications)
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

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

            if (role != null)
            {
                internshipsQuery = internshipsQuery
                    .Where(x => x.Role.ToLower().Contains(role.ToLower()));
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

            if (companyId != null)
            {
                internshipsQuery = internshipsQuery
                    .Where(x => x.Company.Id == companyId);
            }

            if (countryId != null)
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