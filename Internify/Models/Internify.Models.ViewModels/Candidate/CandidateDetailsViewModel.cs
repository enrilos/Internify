namespace Internify.Models.ViewModels.Candidate
{
    public class CandidateDetailsViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string Description { get; set; }

        public string BirthDate { get; set; }

        public string Gender { get; set; }

        public string IsAvailableMessage { get; set; }

        public string Specialization { get; set; }

        // TODO: Candidate's universities listing model
        //public IEnumerable<CandidateUniversityListingViewModel> Universities { get; set; }

        public string Country { get; set; }

        public string Company { get; set; }

        // TODO: Show candidate's reviews.
        //public ICollection<Review> Reviews { get; set; }

        public string CreatedOn { get; init; }

        public string ModifiedOn { get; set; }
    }
}