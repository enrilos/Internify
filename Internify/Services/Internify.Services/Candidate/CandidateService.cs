namespace Internify.Services.Candidate
{
    using Data;

    public class CandidateService : ICandidateService
    {
        private readonly InternifyDbContext data;

        public CandidateService(InternifyDbContext data)
            => this.data = data;

        public bool IsCandidate(string userId)
            => data
            .Candidates
            .Any(x => x.UserId == userId);

        public string GetIdByUserId(string userId)
            => data
            .Candidates
            .Where(x => x.UserId == userId)
            .FirstOrDefault()?.Id;
    }
}
