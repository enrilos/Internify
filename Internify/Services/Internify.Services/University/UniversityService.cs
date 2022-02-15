namespace Internify.Services.University
{
    using Data;

    public class UniversityService : IUniversityService
    {
        private readonly InternifyDbContext data;

        public UniversityService(InternifyDbContext data)
            => this.data = data;

        public bool IsUniversityByUserId(string userId)
            => data
            .Universities
            .Any(x => x.UserId == userId);

        public string GetIdByUserId(string userId)
            => data
            .Universities
            .Where(x => x.UserId == userId)
            .FirstOrDefault()?.Id;
    }
}
