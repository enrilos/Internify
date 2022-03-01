namespace Internify.Data.Models
{
    using Internify.Data.Common;

    public class CandidateUniversity : IAuditInfo, IDeletableEntity
    {
        public string CandidateId { get; init; }

        public Candidate Candidate { get; set; }

        public string UniversityId { get; init; }

        public University University { get; set; }

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}