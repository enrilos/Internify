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

        public bool Delete(string id);

        public UniversityDetailsViewModel GetDetailsModel(string id);

        public EditUniversityFormModel GetEditModel(string id);

        public UniversityListingQueryModel All(
            string name,
            string countryId,
            int currentPage,
            int universitiesPerPage);

        public UniversityAlumniListingQueryModel Alumni(
            string id,
            int currentPage,
            int alumniPerPage);
    }
}