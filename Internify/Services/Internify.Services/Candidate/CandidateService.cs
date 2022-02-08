namespace Internify.Services.Candidate
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Models.ViewModels.Candidate;

    public class CandidateService : ICandidateService
    {
        private readonly InternifyDbContext data;
        private readonly IConfigurationProvider mapper;

        public CandidateService(
            InternifyDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
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

        public IEnumerable<CandidateListingViewModel> All(
            bool isAvailableFilter = true,
            string specializationFilter = "",
            string countryFilter = "")
            => data
            .Candidates
            .Where(x => x.IsAvailable == isAvailableFilter)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ProjectTo<CandidateListingViewModel>(mapper)
            .ToList();
    }
}
