namespace Internify.Services.Internship
{
    using Data;
    using Data.Models;
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
            throw new NotImplementedException();
        }
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
        {
            throw new NotImplementedException();
        }

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
            int companiesPerPage)
        {
            throw new NotImplementedException();
        }
    }
}