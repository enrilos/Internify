namespace Internify.Services.Candidate
{
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Ganss.XSS;
    using Internify.Models.ViewModels.Intern;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.Candidate;
    using Models.InputModels.Intern;
    using Models.ViewModels.Candidate;
    using Models.ViewModels.University;

    using static Common.GlobalConstants;

    public class CandidateService : ICandidateService
    {
        private readonly InternifyDbContext data;
        private readonly UserManager<ApplicationUser> userManager;

        public CandidateService(
            InternifyDbContext data,
            UserManager<ApplicationUser> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public bool IsCandidate(string id)
            => data
            .Candidates
            .Any(x => x.Id == id && !x.IsDeleted);

        public bool IsCandidateByUserId(string userId)
            => data
            .Candidates
            .Any(x => x.UserId == userId && !x.IsDeleted);

        public string GetIdByUserId(string userId)
            => data
            .Candidates
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .FirstOrDefault()?.Id;

        public string Add(
            string userId,
            string firstName,
            string lastName,
            string imageUrl,
            string websiteUrl,
            string description,
            DateTime birthDate,
            Gender gender,
            string specializationId,
            string countryId,
            string hostName)
        {
            var sanitizer = new HtmlSanitizer();
            var sanitizedDescription = sanitizer.Sanitize(description);

            var candidate = new Candidate
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                UserId = userId,
                ImageUrl = string.IsNullOrEmpty(imageUrl) ? Path.Combine(hostName, "/images/avatar.png") : imageUrl.Trim(),
                WebsiteUrl = websiteUrl?.Trim(),
                Description = sanitizedDescription?.Trim(),
                BirthDate = birthDate,
                Gender = gender,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            data.Candidates.Add(candidate);

            Task.Run(async () =>
            {
                var user = await userManager.FindByIdAsync(userId);
                await userManager.AddToRoleAsync(user, CandidateRoleName);
            })
            .GetAwaiter()
            .GetResult();

            data.SaveChanges();

            return candidate.Id;
        }

        public bool Edit(
            string id,
            string firstName,
            string lastName,
            string imageUrl,
            string websiteUrl,
            string description,
            DateTime birthDate,
            Gender gender,
            bool isAvailable,
            string specializationId,
            string countryId)
        {
            var candidate = data.Candidates.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (candidate == null)
            {
                return false;
            }

            var sanitizer = new HtmlSanitizer();
            var sanitizedDescription = sanitizer.Sanitize(description);

            candidate.FirstName = firstName.Trim();
            candidate.LastName = lastName.Trim();
            candidate.ImageUrl = imageUrl?.Trim();
            candidate.WebsiteUrl = websiteUrl?.Trim();
            candidate.Description = sanitizedDescription?.Trim();
            candidate.BirthDate = birthDate;
            candidate.Gender = gender;
            candidate.IsAvailable = isAvailable;
            candidate.SpecializationId = specializationId;
            candidate.CountryId = countryId;

            candidate.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var candidate = data
                .Candidates
                .Include(x => x.Universities)
                .Include(x => x.Applications)
                .Include(x => x.Comments)
                .Include(x => x.Reviews)
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (candidate == null)
            {
                return false;
            }

            candidate.IsDeleted = true;
            candidate.DeletedOn = DateTime.UtcNow;

            foreach (var candidateUniversity in candidate.Universities)
            {
                data.CandidateUniversities.Remove(candidateUniversity);
            }

            foreach (var application in candidate.Applications)
            {
                application.IsDeleted = true;
                application.DeletedOn = candidate.DeletedOn;
            }

            foreach (var comment in candidate.Comments)
            {
                comment.IsDeleted = true;
                comment.DeletedOn = candidate.DeletedOn;
            }

            foreach (var review in candidate.Reviews)
            {
                review.IsDeleted = true;
                review.DeletedOn = candidate.DeletedOn;
            }

            Task.Run(async () =>
            {
                var user = await userManager.FindByIdAsync(candidate.UserId);
                user.IsDeleted = candidate.IsDeleted;
                user.DeletedOn = candidate.DeletedOn;
            })
            .GetAwaiter()
            .GetResult();

            data.SaveChanges();

            return true;
        }

        public bool IsCandidateAlreadyAnIntern(string id)
            => data
            .Candidates
            .Any(x =>
            x.Id == id
            && x.CompanyId != null
            && !x.IsDeleted);

        public bool IsCandidateInCompany(
            string candidateId,
            string companyId)
            => data
            .Candidates
            .Any(x =>
            x.Id == candidateId
            && x.CompanyId == companyId
            && !x.IsDeleted);

        public bool RemoveFromCompany(string candidateId)
        {
            var candidate = data
                .Candidates
                .FirstOrDefault(x => x.Id == candidateId && !x.IsDeleted);

            if (candidate == null)
            {
                return false;
            }

            candidate.CompanyId = null;
            candidate.InternshipRole = null;

            data.SaveChanges();

            return true;
        }

        public bool Exists(string id)
            => data
            .Candidates
            .Any(x =>
            x.Id == id
            && !x.IsDeleted);

        public CandidateDetailsViewModel GetDetailsModel(string id)
            => data
            .Candidates
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new CandidateDetailsViewModel
            {
                Id = id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Description = x.Description,
                BirthDate = x.BirthDate.ToString("d"),
                Gender = x.Gender.ToString(),
                IsAvailableMessage = x.IsAvailable == true ? "✔️ Open for offers" : "❌ Currently not available",
                Specialization = x.Specialization.Name,
                Universities = x.Universities
                    .Select(x => new UniversityListingViewModel
                    {
                        Id = x.University.Id,
                        Name = x.University.Name,
                        ImageUrl = x.University.ImageUrl,
                        Country = x.University.Country.Name
                    })
                    .ToList(),
                CompanyId = x.CompanyId,
                Country = x.Country.Name,
                Company = x.Company == null ? "" : x.Company.Name,
                CreatedOn = x.CreatedOn.ToString("d"),
                ModifiedOn = x.ModifiedOn == null ? null : ((DateTime)x.ModifiedOn).ToShortDateString()
            })
            .FirstOrDefault();

        public EditCandidateFormModel GetEditModel(string id)
            => data
            .Candidates
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new EditCandidateFormModel
            {
                Id = id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Description = x.Description,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                IsAvailable = x.IsAvailable,
                SpecializationId = x.SpecializationId,
                CountryId = x.CountryId,
            })
            .FirstOrDefault();

        public CandidateListingQueryModel All(
            string firstName,
            string lastName,
            string specializationId,
            string countryId,
            bool isAvailable,
            int currentPage,
            int candidatesPerPage)
        {
            var candidatesQuery = data
                .Candidates
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (isAvailable)
            {
                candidatesQuery = candidatesQuery
                    .Where(x => x.IsAvailable);
            }
            // Otherwise, list all despite availability.

            if (firstName != null)
            {
                candidatesQuery = candidatesQuery
                    .Where(x => x.FirstName.ToLower().Contains(firstName.ToLower().Trim()));
            }

            if (lastName != null)
            {
                candidatesQuery = candidatesQuery
                    .Where(x => x.LastName.ToLower().Contains(lastName.ToLower().Trim()));
            }

            if (specializationId != null)
            {
                candidatesQuery = candidatesQuery
                    .Where(x => x.SpecializationId == specializationId);
            }

            if (countryId != null)
            {
                candidatesQuery = candidatesQuery
                    .Where(x => x.CountryId == countryId);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (candidatesPerPage < 6)
            {
                candidatesPerPage = 6;
            }
            else if (candidatesPerPage > 96)
            {
                candidatesPerPage = 96;
            }

            var candidates = candidatesQuery
               .OrderByDescending(x => x.CreatedOn)
               .ThenBy(x => x.FirstName)
               .ThenBy(x => x.LastName)
               .Skip((currentPage - 1) * candidatesPerPage)
               .Take(candidatesPerPage)
               .Select(x => new CandidateListingViewModel
               {
                   Id = x.Id,
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   ImageUrl = x.ImageUrl,
                   Age = (int)((DateTime.Now - x.BirthDate).TotalDays / DaysInAYear),
                   Country = x.Country.Name,
                   Specialization = x.Specialization.Name
               })
               .ToList();

            return new CandidateListingQueryModel
            {
                FirstName = firstName,
                LastName = lastName,
                SpecializationId = specializationId,
                CountryId = countryId,
                IsAvailable = isAvailable,
                Candidates = candidates,
                CurrentPage = currentPage,
                CandidatesPerPage = candidatesPerPage,
                TotalCandidates = candidatesQuery.Count()
            };
        }

        public InternListingQueryModel GetCandidatesByCompany(
            string companyId,
            string firstName,
            string lastName,
            string internshipRole,
            int currentPage,
            int internsPerPage)
        {
            var internsQuery = data
                .Candidates
                .Where(x =>
                x.CompanyId == companyId
                && !x.IsDeleted)
                .AsQueryable();

            if (firstName != null)
            {
                internsQuery = internsQuery
                    .Where(x => x.FirstName.ToLower().Contains(firstName.ToLower().Trim()));
            }

            if (lastName != null)
            {
                internsQuery = internsQuery
                    .Where(x => x.LastName.ToLower().Contains(lastName.ToLower().Trim()));
            }

            if (internshipRole != null)
            {
                internsQuery = internsQuery
                    .Where(x => x.InternshipRole.ToLower().Contains(internshipRole.ToLower().Trim()));
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (internsPerPage < 6)
            {
                internsPerPage = 6;
            }
            else if (internsPerPage > 96)
            {
                internsPerPage = 96;
            }

            var interns = internsQuery
               .OrderByDescending(x => x.CreatedOn)
               .ThenBy(x => x.FirstName)
               .ThenBy(x => x.LastName)
               .Skip((currentPage - 1) * internsPerPage)
               .Take(internsPerPage)
               .Select(x => new InternListingViewModel
               {
                   Id = x.Id,
                   FullName = x.FirstName + " " + x.LastName,
                   ImageUrl = x.ImageUrl,
                   Age = (int)((DateTime.Now - x.BirthDate).TotalDays / DaysInAYear),
                   InternshipRole = x.InternshipRole
               })
               .ToList();

            return new InternListingQueryModel
            {
                CompanyId = companyId,
                FirstName = firstName,
                LastName = lastName,
                InternshipRole = internshipRole,
                Interns = interns,
                CurrentPage = currentPage,
                InternsPerPage = internsPerPage,
                TotalInterns = internsQuery.Count()
            };
        }
    }
}