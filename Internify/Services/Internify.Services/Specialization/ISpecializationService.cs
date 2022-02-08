namespace Internify.Services.Specialization
{
    using Models.ViewModels.Specialization;

    public interface ISpecializationService
    {
        bool Exists(string id);

        IEnumerable<SpecializationListingViewModel> All();
    }
}