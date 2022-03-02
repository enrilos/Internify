namespace Internify.Services.CandidateUniversity
{
    using Data;
    using Data.Models;

    public class CandidateUniversityService : ICandidateUniversityService
    {
        private readonly InternifyDbContext data;

        public CandidateUniversityService(InternifyDbContext data)
            => this.data = data;

        public bool IsCandidateInUniversityAlumni(string universityId, string candidateId)
            => data
            .CandidateUniversities
            .Any(x =>
            x.UniversityId == universityId
            && x.CandidateId == candidateId);

        public bool AddCandidateToAlumni(string universityId, string candidateId)
        {
            var university = data.Universities.FirstOrDefault(x => x.Id == universityId && !x.IsDeleted);

            if (university == null)
            {
                return false;
            }

            var candidate = data.Candidates.FirstOrDefault(x => x.Id == candidateId && !x.IsDeleted);

            if (candidate == null)
            {
                return false;
            }

            var candidateUniversity = new CandidateUniversity
            {
                CandidateId = candidate.Id,
                UniversityId = university.Id
            };

            data.CandidateUniversities.Add(candidateUniversity);

            data.SaveChanges();

            return true;
        }

        public bool RemoveCandidateFromAlumni(string universityId, string candidateId)
        {
            var candidateUniversity = data
                .CandidateUniversities
                .FirstOrDefault(x =>
                x.UniversityId == universityId
                && x.CandidateId == candidateId);

            if (candidateUniversity == null)
            {
                return false;
            }

            data.CandidateUniversities.Remove(candidateUniversity);

            data.SaveChanges();

            return true;
        }
    }
}