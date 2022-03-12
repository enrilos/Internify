namespace Internify.Services.Application
{
    public interface IApplicationService
    {
        public bool Add(
            string internshipId,
            string candidateId,
            string coverLetter);

        public bool HasCandidateApplied(
            string candidateId,
            string internshipId);
    }
}