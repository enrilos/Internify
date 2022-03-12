namespace Internify.Services.Application
{
    using Data;
    using Data.Models;

    public class ApplicationService : IApplicationService
    {
        private readonly InternifyDbContext data;

        public ApplicationService(InternifyDbContext data)
            => this.data = data;

        public bool Add(
            string internshipId,
            string candidateId,
            string coverLetter)
        {
            var application = new Application
            {
                InternshipId = internshipId,
                CandidateId = candidateId,
                CoverLetter = coverLetter
            };

            data.Applications.Add(application);

            var result = data.SaveChanges();

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        public bool HasCandidateApplied(
            string candidateId,
            string internshipId)
            => data
            .Applications
            .Any(x =>
            x.CandidateId == candidateId
            && x.InternshipId == internshipId
            && !x.IsDeleted);
    }
}