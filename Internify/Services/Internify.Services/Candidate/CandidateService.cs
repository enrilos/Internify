namespace Internify.Services.Candidate
{
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Models.InputModels.Candidate;
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

        public CandidateListingQueryModel All(
            string firstName,
            string lastName,
            string specializationId,
            string countryId,
            bool isAvailable,
            int currentPage,
            int candidatesPerPage)
        {
            var candidatesQuery = data.Candidates.AsQueryable();

            if (isAvailable)
            {
                candidatesQuery = candidatesQuery.Where(x => x.IsAvailable == isAvailable);
            }
            // Otherwise, list all despite availability.

            if (firstName != null)
            {
                candidatesQuery = candidatesQuery
                    .Where(x => x.FirstName.Contains(firstName.Trim()));
            }

            if (lastName != null)
            {
                candidatesQuery = candidatesQuery
                    .Where(x => x.LastName.Contains(lastName.Trim()));
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
                   Age = (int)((DateTime.Now - x.BirthDate).TotalDays / 365.242199),
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
    }
}