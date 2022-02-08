namespace Internify.Services.Candidate
{
    public interface ICandidateService
    {
        public bool IsCandidate(string userId);

        public string GetIdByUserId(string userId);
    }
}
