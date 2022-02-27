namespace Internify.Services.University
{
    using Models.InputModels.University;
    using Models.ViewModels.University;

    public interface IUniversityService
    {
        public bool IsUniversity(string id);

        public bool IsUniversityByUserId(string userId);

        public string GetIdByUserId(string userId);

        public string Add(
            string userId,
            string name,
            string imageUrl,
            string websiteUrl,
            string description,
            string countryId);

        public bool Edit(
            string id,
            string name,
            string imageUrl,
            string websiteUrl,
            string description,
            string countryId);

        public UniversityDetailsViewModel GetDetailsModel(string id);

        public EditUniversityFormModel GetEditModel(string id);

        public IEnumerable<UniversityListingViewModel> All();
    }
}