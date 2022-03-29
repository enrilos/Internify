namespace Internify.Services.Data.Country
{
    using Models.ViewModels.Country;

    public interface ICountryService
    {
        bool Exists(string id);

        IEnumerable<CountryListingViewModel> All();
    }
}
