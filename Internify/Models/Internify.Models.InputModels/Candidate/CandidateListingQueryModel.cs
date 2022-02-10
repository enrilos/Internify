namespace Internify.Models.InputModels.Candidate
{
    public class CandidateListingQueryModel
    {
        public string FullName { get; init; }

        public bool IsAvailable { get; init; }

        public string SpecializationId { get; init; }

        public string CountryId { get; init; }
    }
}
