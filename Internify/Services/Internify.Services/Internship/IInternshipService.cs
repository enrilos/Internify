namespace Internify.Services.Internship
{
    using Models.InputModels.Internship;
    using Models.ViewModels.Internship;

    public interface IInternshipService
    {
        public string Add(
            string companyId,
            string role,
            bool isPaid,
            decimal? salaryUSD,
            bool isRemote,
            string description,
            string countryId);

        public bool Edit(
            string id,
            bool isPaid,
            decimal? salaryUSD,
            string description);

        public bool Delete(string id);

        public bool Exists(string id);

        public bool IsInternshipOwnedByCompany(
            string internshipId,
            string companyId);

        public InternshipDetailsViewModel GetDetailsModel(string id);

        public EditInternshipFormModel GetEditModel(string id);

        public InternshipListingQueryModel All(
            string role,
            bool isPaid,
            bool isRemote,
            string companyId,
            string countryId,
            int currentPage,
            int internshipsPerPage);
    }
}