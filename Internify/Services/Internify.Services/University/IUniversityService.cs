namespace Internify.Services.University
{
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
    }
}