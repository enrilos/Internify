namespace Internify.Models.ViewModels.Candidate
{
    public class CandidateDetailsViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string BirthDate { get; set; }

        public string Gender { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string Specialization { get; set; }

        public string University { get; set; }

        public string Country { get; set; }

        public string Company { get; set; }

        // TODO: Show candidate's reviews.
        //public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public string CreatedOn { get; init; }

        public string ModifiedOn { get; set; }
    }
}