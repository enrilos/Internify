namespace Internify.Services.University
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.Candidate;
    using Models.InputModels.University;
    using Models.ViewModels.Candidate;
    using Models.ViewModels.University;

    using static Common.GlobalConstants;

    public class UniversityService : IUniversityService
    {
        private readonly InternifyDbContext data;
        private readonly UserManager<ApplicationUser> userManager;

        public UniversityService(
            InternifyDbContext data,
            UserManager<ApplicationUser> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public bool IsUniversity(string id)
            => data
            .Universities
            .Any(x => x.Id == id && !x.IsDeleted);

        public bool IsUniversityByUserId(string userId)
            => data
            .Universities
            .Any(x => x.UserId == userId && !x.IsDeleted);

        public string GetIdByUserId(string userId)
            => data
            .Universities
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .FirstOrDefault()?.Id;

        public string Add(
            string userId,
            string name,
            string imageUrl,
            string websiteUrl,
            int founded,
            string description,
            string countryId)
        {
            var university = new University
            {
                UserId = userId,
                Name = name.Trim(),
                ImageUrl = imageUrl.Trim(),
                WebsiteUrl = websiteUrl.Trim(),
                Founded = founded,
                Description = description.Trim(),
                CountryId = countryId
            };

            data.Universities.Add(university);

            Task.Run(async () =>
            {
                var user = await userManager.FindByIdAsync(userId);
                await userManager.AddToRoleAsync(user, UniversityRoleName);
            })
            .GetAwaiter()
            .GetResult();

            data.SaveChanges();

            return university.Id;
        }

        public bool Edit(
            string id,
            string name,
            string imageUrl,
            string websiteUrl,
            int founded,
            string description,
            string countryId)
        {
            var university = data.Universities.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (university == null)
            {
                return false;
            }

            university.Name = name;
            university.ImageUrl = imageUrl;
            university.WebsiteUrl = websiteUrl;
            university.Founded = founded;
            university.Description = description;
            university.CountryId = countryId;

            university.ModifiedOn = DateTime.UtcNow;

            data.SaveChanges();

            return true;
        }

        public bool Delete(string id)
        {
            var university = data
                .Universities
                .Include(x => x.Alumni)
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (university == null)
            {
                return false;
            }

            university.IsDeleted = true;
            university.DeletedOn = DateTime.UtcNow;

            foreach (var candidateUniversity in university.Alumni)
            {
                data.CandidateUniversities.Remove(candidateUniversity);
            }

            Task.Run(async () =>
            {
                var user = await userManager.FindByIdAsync(university.UserId);
                user.IsDeleted = university.IsDeleted;
                user.DeletedOn = university.DeletedOn;
            })
            .GetAwaiter()
            .GetResult();

            data.SaveChanges();

            return true;
        }

        public bool Exists(string id)
            => data
            .Universities
            .Any(x => x.Id == id && !x.IsDeleted);

        public UniversityDetailsViewModel GetDetailsModel(string id)
            => data
            .Universities
            .Where(x => x.Id == id && !x.IsDeleted)
            .Include(x => x.Alumni)
            .Select(x => new UniversityDetailsViewModel
            {
                Id = id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Founded = x.Founded,
                Description = x.Description,
                Country = x.Country.Name,
                HasAlumni = x.Alumni.Any()
            })
            .FirstOrDefault();

        public EditUniversityFormModel GetEditModel(string id)
            => data
            .Universities
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new EditUniversityFormModel
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Founded = x.Founded,
                Description = x.Description,
                CountryId = x.CountryId
            })
            .FirstOrDefault();

        public UniversityListingQueryModel All(
            string name,
            string countryId,
            int currentPage,
            int universitiesPerPage)
        {
            var universitiesQuery = data.Universities.Where(x => !x.IsDeleted).AsQueryable();

            if (name != null)
            {
                universitiesQuery = universitiesQuery
                    .Where(x => x.Name.Contains(name.Trim()));
            }

            if (countryId != null)
            {
                universitiesQuery = universitiesQuery
                    .Where(x => x.CountryId == countryId);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (universitiesPerPage < 6)
            {
                universitiesPerPage = 6;
            }
            else if (universitiesPerPage > 96)
            {
                universitiesPerPage = 96;
            }

            var universities = universitiesQuery
                .OrderByDescending(x => x.CreatedOn)
                .ThenBy(x => x.Name)
                .Skip((currentPage - 1) * universitiesPerPage)
                .Take(universitiesPerPage)
                .Select(x => new UniversityListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Country = x.Country.Name
                })
                .ToList();

            return new UniversityListingQueryModel
            {
                Name = name,
                CountryId = countryId,
                Universities = universities,
                CurrentPage = currentPage,
                UniversitiesPerPage = universitiesPerPage,
                TotalUniversities = universitiesQuery.Count()
            };
        }

        public CandidateListingQueryModel Alumni(
            string universityId,
            string firstName,
            string lastName,
            string specializationId,
            string countryId,
            bool isAvailable,
            int currentPage,
            int alumniPerPage)
        {
            var alumniQuery = data
                .Candidates
                .Where(x => x.Universities.Any(u => u.UniversityId == universityId && !u.University.IsDeleted))
                .AsQueryable();

            if (isAvailable)
            {
                alumniQuery = alumniQuery
                    .Where(x => x.IsAvailable);
            }

            if (firstName != null)
            {
                alumniQuery = alumniQuery
                    .Where(x => x.FirstName.Contains(firstName.Trim()));
            }

            if (lastName != null)
            {
                alumniQuery = alumniQuery
                    .Where(x => x.LastName.Contains(lastName.Trim()));
            }

            if (specializationId != null)
            {
                alumniQuery = alumniQuery
                    .Where(x => x.SpecializationId == specializationId);
            }

            if (countryId != null)
            {
                alumniQuery = alumniQuery
                    .Where(x => x.CountryId == countryId);
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (alumniPerPage < 6)
            {
                alumniPerPage = 6;
            }
            else if (alumniPerPage > 96)
            {
                alumniPerPage = 96;
            }

            var candidates = alumniQuery
               .OrderByDescending(x => x.CreatedOn)
               .ThenBy(x => x.FirstName)
               .ThenBy(x => x.LastName)
               .Skip((currentPage - 1) * alumniPerPage)
               .Take(alumniPerPage)
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

            var universityName = data.Universities.Find(universityId)?.Name;

            return new CandidateListingQueryModel
            {
                UniversityId = universityId,
                UniversityName = universityName,
                FirstName = firstName,
                LastName = lastName,
                SpecializationId = specializationId,
                CountryId = countryId,
                IsAvailable = isAvailable,
                Candidates = candidates,
                CurrentPage = currentPage,
                CandidatesPerPage = alumniPerPage,
                TotalCandidates = alumniQuery.Count()
            };
        }
    }
}