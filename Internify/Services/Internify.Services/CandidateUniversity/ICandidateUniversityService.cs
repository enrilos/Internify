namespace Internify.Services.CandidateUniversity
{
    public interface ICandidateUniversityService
    {
        public bool IsCandidateInUniversityAlumni(string universityId, string candidateId);

        public bool AddCandidateToAlumni(string universityId, string candidateId);

        public bool RemoveCandidateFromAlumni(string universityId, string candidateId);
    }
}