namespace Internify.Services.Candidate
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Models.ViewModels.Candidate;

    public class CandidateService : ICandidateService
    {
        private readonly InternifyDbContext data;
        private readonly IMapper mapper;

        public CandidateService(
            InternifyDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
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

        public CandidateDetailsViewModel Get(string id)
        {
            var candidate = data.Candidates.Find(id);
            var mappedCandidate = mapper.Map<CandidateDetailsViewModel>(candidate);

            return mappedCandidate;
        }

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
            .ProjectTo<CandidateListingViewModel>(mapper.ConfigurationProvider)
            .ToList();
    }
}
