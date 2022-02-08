namespace Internify.Services.University
{
    public interface IUniversityService
    {
        public bool IsUniversity(string userId);

        public string GetIdByUserId(string userId);
    }
}
