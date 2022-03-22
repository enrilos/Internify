namespace Internify.Services.University
{
    using Data.Models.Enums;
    using Models.InputModels.Candidate;
    using Models.InputModels.University;
    using Models.ViewModels.University;

    public interface IUniversityService
    {
        bool IsUniversity(string id);

        bool IsUniversityByUserId(string userId);

        string GetIdByUserId(string userId);

        int TotalCount();

        string Add(
           string userId,
           string name,
           string imageUrl,
           string websiteUrl,
           int founded,
           Type type,
           string description,
           string countryId);

        bool Edit(
           string id,
           string name,
           string imageUrl,
           string websiteUrl,
           int founded,
           Type type,
           string description,
           string countryId);

        bool Delete(string id);

        bool Exists(string id);

        UniversityDetailsViewModel GetDetailsModel(string id);

        EditUniversityFormModel GetEditModel(string id);

        UniversityListingQueryModel All(
           string name,
           Type? type,
           string countryId,
           int currentPage,
           int universitiesPerPage);

        CandidateListingQueryModel Alumni(
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