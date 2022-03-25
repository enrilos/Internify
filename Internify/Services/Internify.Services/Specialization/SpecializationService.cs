namespace Internify.Services.Specialization
{
    using Data;
    using Models.ViewModels.Specialization;

    public class SpecializationService : ISpecializationService
    {
        private readonly InternifyDbContext data;

        public SpecializationService(InternifyDbContext data)
            => this.data = data;

        public IEnumerable<SpecializationListingViewModel> All()
            => data
            .Specializations
            .OrderBy(x => x.Name)
            .Select(x => new SpecializationListingViewModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();

        public bool Exists(string id)
            => data
            .Specializations
            .FirstOrDefault(x => x.Id == id) != null;
    }
}