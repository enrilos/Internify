namespace Internify.Models.ViewModels.Candidate
{
    using University;

    public class CandidateDetailsViewModel
    {
        public string Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string ImageUrl { get; init; }

        public string WebsiteUrl { get; init; }

        public string Description { get; init; }

        public string BirthDate { get; init; }

        public string Gender { get; init; }

        public string IsAvailableMessage { get; init; }

        public string Specialization { get; init; }

        public IEnumerable<UniversityListingViewModel> Universities { get; init; }

        public string Country { get; init; }

        public string Company { get; init; }

        public string CreatedOn { get; init; }

        public string ModifiedOn { get; init; }
    }
}