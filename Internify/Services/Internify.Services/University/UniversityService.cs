namespace Internify.Services.University
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
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

        public UniversityDetailsViewModel GetDetailsModel(string id)
            => data
            .Universities
            .Where(x => x.Id == id)
            .Select(x => new UniversityDetailsViewModel
            {
                Id = id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                Description = x.Description,
                Country = x.Country.Name,
                //Alumni = x.Alumni -- TODO
            })
            .FirstOrDefault();

        public IEnumerable<UniversityListingViewModel> All()
            => data
            .Universities
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