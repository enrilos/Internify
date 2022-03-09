namespace Internify.Services.University
{
    using Data.Models.Enums;
    using Models.InputModels.Candidate;
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
            int founded,
            Type type,
            string description,
            string countryId);

        public bool Edit(
            string id,
            string name,
            string imageUrl,
            string websiteUrl,
            int founded,
            Type type,
            string description,
            string countryId);

        public bool Delete(string id);

        public bool Exists(string id);

        public UniversityDetailsViewModel GetDetailsModel(string id);

        public EditUniversityFormModel GetEditModel(string id);

        public UniversityListingQueryModel All(
            string name,
            Type? type,
            string countryId,
            int currentPage,
            int universitiesPerPage);

        public CandidateListingQueryModel Alumni(
            string universityId,
            string firstName,
            string lastName,
            string specializationId,
            string countryId,
            bool isAvailable,
            int currentPage,
            int alumniPerPage);
    }
}