namespace Internify.Services.Country
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Internify.Data;
    using Models.ViewModels.Country;

    public class CountryService : ICountryService
    {
        private readonly InternifyDbContext data;
        private readonly IConfigurationProvider mapper;

        public CountryService(
            InternifyDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<CountryListingViewModel> All()
            => data
            .Countries
            .OrderBy(x => x.Name)
            .ProjectTo<CountryListingViewModel>(mapper)
            .ToList();

        public bool Exists(string id)
            => data
            .Countries
            .FirstOrDefault(x => x.Id == id) != null;
    }
}
