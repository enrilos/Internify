namespace Internify.Services.Specialization
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Models.ViewModels.Specialization;

    public class SpecializationService : ISpecializationService
    {
        private readonly InternifyDbContext data;
        private readonly IConfigurationProvider mapper;

        public SpecializationService(
            InternifyDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<SpecializationListingViewModel> All()
            => data
            .Specializations
            .OrderBy(x => x.Name)
            .ProjectTo<SpecializationListingViewModel>(mapper)
            .ToList();

        public bool Exists(string id)
            => data
            .Specializations
            .FirstOrDefault(x => x.Id == id) != null;
    }
}