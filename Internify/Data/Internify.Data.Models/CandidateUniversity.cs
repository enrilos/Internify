namespace Internify.Data.Models
{
    public class CandidateUniversity
    {
        public string CandidateId { get; init; }

        public Candidate Candidate { get; set; }

        public string UniversityId { get; init; }

        public University University { get; set; }
    }
}