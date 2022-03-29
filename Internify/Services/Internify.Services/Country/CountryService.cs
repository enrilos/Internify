namespace Internify.Services.Data.Country
{
    using Internify.Data;
    using Models.ViewModels.Country;

    public class CountryService : ICountryService
    {
        private readonly InternifyDbContext data;

        public CountryService(InternifyDbContext data)
            => this.data = data;

        public IEnumerable<CountryListingViewModel> All()
            => data
            .Countries
            .OrderBy(x => x.Name)
            .Select(x => new CountryListingViewModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();

        public bool Exists(string id)
            => data
            .Countries
            .FirstOrDefault(x => x.Id == id) != null;
    }
}