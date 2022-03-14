﻿namespace Internify.Services.Application
{
    using Data;
    using Data.Models;
    using Models.InputModels.Application;
    using Models.InputModels.Internship;
    using Models.ViewModels.Application;
    using Models.ViewModels.Internship;

    using static Common.GlobalConstants;

    public class ApplicationService : IApplicationService
    {
        private readonly InternifyDbContext data;
        private const int CoverLetterSegmentNumber = 65;

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
                CoverLetter = coverLetter.Trim()
            };

            data.Applications.Add(application);

            var result = data.SaveChanges();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public string Edit(
            string id,
            string coverLetter)
        {
            var application = data.Applications.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (application == null)
            {
                return null;
            }

            application.CoverLetter = coverLetter.Trim();
            application.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return id;
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

        public bool Exists(string id)
            => data
            .Applications
            .Any(x =>
            x.Id == id
            && !x.IsDeleted);

        public bool IsApplicationOwnedByCandidate(
            string applicationId,
            string candidateId)
            => data
            .Applications
            .Any(x =>
            x.Id == applicationId
            && x.CandidateId == candidateId
            && !x.IsDeleted);

        public bool Delete(string id)
        {
            var application = data.Applications.Find(id);

            if (application == null)
            {
                return false;
            }

            application.IsDeleted = true;
            application.DeletedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public ApplicationDetailsViewModel GetDetailsModel(string id)
            => data
            .Applications
            .Where(x =>
            x.Id == id
            && !x.IsDeleted)
            .Select(x => new ApplicationDetailsViewModel
            {
                Id = x.Id,
                InternshipId = x.InternshipId,
                Role = x.Internship.Role,
                CompanyId = x.Internship.CompanyId,
                Company = x.Internship.Company.Name,
                CoverLetter = x.CoverLetter,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn
            })
            .FirstOrDefault();

        public ApplicationFormModel GetEditModel(string id)
            => data
            .Applications
            .Where(x =>
            x.Id == id
            && !x.IsDeleted)
            .Select(x => new ApplicationFormModel
            {
                Id = x.Id,
                InternshipId = x.InternshipId,
                CandidateId = x.CandidateId,
                CoverLetter = x.CoverLetter
            })
            .FirstOrDefault();

        public InternshipApplicantListingQueryModel GetInternshipApplicants(
            string internshipId,
            int currentPage,
            int applicantsPerPage)
        {
            var internshipApplicantsQuery = data
                .Applications
                .Where(x =>
                x.InternshipId == internshipId
                && !x.IsDeleted)
                .AsQueryable();

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (applicantsPerPage < 6)
            {
                applicantsPerPage = 6;
            }
            else if (applicantsPerPage > 96)
            {
                applicantsPerPage = 96;
            }

            var internshipApplicants = internshipApplicantsQuery
                .OrderByDescending(x => x.CreatedOn)
                .ThenBy(x => x.Candidate.FirstName + " " + x.Candidate.LastName)
                .Skip((currentPage - 1) * applicantsPerPage)
                .Take(applicantsPerPage)
                .Select(x => new InternshipApplicantListingViewModel
                {
                    ApplicationId = x.Id,
                    CandidateFullName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                    CandidateImageUrl = x.Candidate.ImageUrl,
                    CandidateAge = (int)((DateTime.Now - x.Candidate.BirthDate).TotalDays / DaysInAYear),
                    CandidateCoverLetterSegment = x.CoverLetter.Length > CoverLetterSegmentNumber
                    ? x.CoverLetter.Substring(0, CoverLetterSegmentNumber) + "..."
                    : x.CoverLetter.Substring(0, CoverLetterSegmentNumber)
                })
               .ToList();

            return new InternshipApplicantListingQueryModel
            {
                InternshipId = internshipId,
                CurrentPage = currentPage,
                ApplicantsPerPage = applicantsPerPage,
                Applicants = internshipApplicants,
                TotalApplicants = internshipApplicantsQuery.Count()
            };
        }

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
                    .Where(x => x.Internship.Role.ToLower().Contains(role.ToLower().Trim()));
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