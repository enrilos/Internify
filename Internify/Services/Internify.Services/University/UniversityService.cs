namespace Internify.Services.University
{
    using Data;

    public class UniversityService : IUniversityService
    {
        private readonly InternifyDbContext data;

        public UniversityService(InternifyDbContext data)
            => this.data = data;

        public bool IsUniversity(string id)
            => data
            .Universities
            .Any(x => x.Id == id && !x.IsDeleted);

        public bool IsUniversityByUserId(string userId)
            => data
            .Universities
            .Any(x => x.UserId == userId && !x.IsDeleted);

        public string GetIdByUserId(string userId)
            => data
            .Universities
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .FirstOrDefault()?.Id;
    }
}