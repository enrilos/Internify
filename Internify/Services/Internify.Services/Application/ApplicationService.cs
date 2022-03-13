namespace Internify.Services.Application
{
    using Data;
    using Data.Models;
    using Models.InputModels.Application;
    using Models.ViewModels.Application;

    public class ApplicationService : IApplicationService
    {
        private readonly InternifyDbContext data;
        private const int CoverLetterSegmentNumber = 150;

        public ApplicationService(InternifyDbContext data)
            => this.data = data;

        public bool Add(
            string internshipId,
            string candidateId,
            string coverLetter)
        {
            var application = new Application
            {
                InternshipId = internshipId,
                CandidateId = candidateId,
                CoverLetter = coverLetter
            };

            data.Applications.Add(application);

            var result = data.SaveChanges();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public bool HasCandidateApplied(
            string candidateId,
            string internshipId)
            => data
            .Applications
            .Any(x =>
            x.CandidateId == candidateId
            && x.InternshipId == internshipId
            && !x.IsDeleted);

        public MyApplicationListingQueryModel GetCandidateApplications(
            string candidateId,
            string role,
            string companyId,
            int currentPage,
            int applicationsPerPage)
        {
            var applicationsQuery = data
                .Applications
                .Where(x =>
                x.CandidateId == candidateId
                && !x.IsDeleted)
                .AsQueryable();

            if (role != null)
            {
                applicationsQuery = applicationsQuery
                    .Where(x => x.Internship.Role.ToLower().Contains(role.ToLower()));
            }

            if (companyId != null)
            {
                applicationsQuery = applicationsQuery
                    .Where(x => x.Internship.CompanyId == companyId);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (applicationsPerPage < 6)
            {
                applicationsPerPage = 6;
            }
            else if (applicationsPerPage > 96)
            {
                applicationsPerPage = 96;
            }

            var applications = applicationsQuery
               .OrderByDescending(x => x.CreatedOn)
               .Skip((currentPage - 1) * applicationsPerPage)
               .Take(applicationsPerPage)
               .Select(x => new MyApplicationListingViewModel
               {
                   Id = x.Id,
                   Role = x.Internship.Role,
                   Company = x.Internship.Company.Name
               })
               .ToList();

            return new MyApplicationListingQueryModel
            {
                CandidateId = candidateId,
                Role = role,
                CompanyId = companyId,
                Applications = applications,
                CurrentPage = currentPage,
                ApplicationsPerPage = applicationsPerPage,
                TotalApplications = applicationsQuery.Count()
            };
        }
    }
}