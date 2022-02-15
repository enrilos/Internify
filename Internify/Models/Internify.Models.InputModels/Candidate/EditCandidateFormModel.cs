namespace Internify.Models.InputModels.Candidate
{
    using Data.Models.Enums;
    using Internify.Models.ViewModels.Country;
    using Internify.Models.ViewModels.Specialization;

    public class EditCandidateFormModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public bool IsAvailable { get; set; }

        public string SpecializationId { get; set; }

        public IEnumerable<SpecializationListingViewModel> Specializations { get; set; }

        // TODO: Being able to edit university
        //public string UniversityId { get; }

        //public ICollection<UniversityListingViewModel> Universities { get; }

        public string CountryId { get; set; }

        public IEnumerable<CountryListingViewModel> Countries { get; set; }
    }
}