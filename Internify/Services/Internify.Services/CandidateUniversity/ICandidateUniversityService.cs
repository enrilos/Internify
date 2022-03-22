namespace Internify.Services.CandidateUniversity
{
    public interface ICandidateUniversityService
    {
        bool IsCandidateInUniversityAlumni(string universityId, string candidateId);

        bool AddCandidateToAlumni(string universityId, string candidateId);

        bool RemoveCandidateFromAlumni(string universityId, string candidateId);
    }
}