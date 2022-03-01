namespace Internify.Services.University
{
    using Data;
    using Data.Models;
    using Internify.Models.ViewModels.Candidate;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.University;
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
            string description,
            string countryId)
        {
            var university = new University
            {
                UserId = userId,
                Name = name,
                ImageUrl = imageUrl.Trim(),
                WebsiteUrl = websiteUrl.Trim(),
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

        public UniversityDetailsViewModel GetDetailsModel(string id)
            => data
            .Universities
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new UniversityDetailsViewModel
            {
                Id = id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Description = x.Description,
                Country = x.Country.Name,
                Alumni = x.Alumni
                .Select(c => new CandidateListingViewModel
                {
                    Id = c.Candidate.Id,
                    FirstName = c.Candidate.LastName,
                    LastName = c.Candidate.LastName,
                    Age = (int)((DateTime.Now - c.Candidate.BirthDate).TotalDays / 365.242199),
                    ImageUrl = c.Candidate.ImageUrl,
                    Country = c.Candidate.Country.Name,
                    Specialization = c.Candidate.Specialization.Name
                })
                .ToList()
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
                Description = x.Description,
                CountryId = x.CountryId
            })
            .FirstOrDefault();

        public IEnumerable<UniversityListingViewModel> All()
            => data
            .Universities
            .Where(x => !x.IsDeleted)
            .Select(x => new UniversityListingViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Country = x.Country.Name
            })
            .ToList();
    }
}