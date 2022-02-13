namespace Internify.Services.Candidate
{
    using Data;
    using Data.Models;
    using Internify.Data.Models.Enums;
    using Models.ViewModels.Candidate;

    public class CandidateService : ICandidateService
    {
        private readonly InternifyDbContext data;

        public CandidateService(InternifyDbContext data)
        {
            this.data = data;
        }

        public bool IsCandidate(string userId)
            => data
            .Candidates
            .Any(x => x.UserId == userId);

        public string GetIdByUserId(string userId)
            => data
            .Candidates
            .Where(x => x.UserId == userId)
            .FirstOrDefault()?.Id;

        public string Add(
            string userId,
            string firstName,
            string lastName,
            string description,
            string imageUrl,
            string websiteUrl,
            DateTime birthDate,
            Gender gender,
            string specializationId,
            string countryId,
            string hostName)
        {
            var candidate = new Candidate
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                UserId = userId,
                Description = description?.Trim(),
                ImageUrl = imageUrl == null ? Path.Combine(hostName, "/images/avatar.png") : imageUrl?.Trim(),
                WebsiteUrl = websiteUrl?.Trim(),
                BirthDate = birthDate,
                Gender = gender,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            data.Candidates.Add(candidate);
            data.SaveChanges();

            return null;
        }

        public CandidateDetailsViewModel Get(string id)
            => data
            .Candidates
            .Where(x => x.Id == id)
            .Select(x => new CandidateDetailsViewModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                WebsiteUrl = x.WebsiteUrl,
                BirthDate = x.BirthDate.ToString("d"),
                Gender = x.Gender.ToString(),
                IsAvailable = x.IsAvailable,
                Specialization = x.Specialization.Name,
                University = x.University == null ? "" : x.University.Name,
                Country = x.Country.Name,
                Company = x.Company == null ? "" : x.Company.Name,
                CreatedOn = x.CreatedOn.ToString("d"),
                ModifiedOn = x.ModifiedOn == null ? null : ((DateTime)x.ModifiedOn).ToShortDateString()
            })
            .FirstOrDefault();

        public IEnumerable<CandidateListingViewModel> All(
            string fullName = "",
            bool isAvailable = true,
            string specializationId = "",
            string countryId = "",
            int currentPage = 1,
            int candidatesPerPage = 2)
                => data
                .Candidates
                //.Where(x =>
                //    (x.FirstName + " " + x.LastName).Contains(fullName.Trim())
                //    && x.IsAvailable == isAvailable
                //    && x.SpecializationId == specializationId
                //    && x.CountryId == countryId)
                //.Skip((currentPage - 1) * candidatesPerPage)
                //.Take(candidatesPerPage)
                .OrderByDescending(x => x.CreatedOn)
                .ThenBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x => new CandidateListingViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ImageUrl = x.ImageUrl,
                    Age = (int)((DateTime.Now - x.BirthDate).TotalDays / 365.242199),
                    Country = x.Country.Name,
                    Specialization = x.Specialization.Name
                })
                .ToList();
    }
}
