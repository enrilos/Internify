namespace Internify.Services.Data.Specialization
{
    using Models.ViewModels.Specialization;

    public interface ISpecializationService
    {
        bool Exists(string id);

        IEnumerable<SpecializationListingViewModel> All();
    }
}