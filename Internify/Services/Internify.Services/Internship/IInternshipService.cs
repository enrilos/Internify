namespace Internify.Services.Internship
{
    using Models.InputModels.Internship;
    using Models.ViewModels.Internship;

    public interface IInternshipService
    {
        string Add(
           string companyId,
           string role,
           bool isPaid,
           decimal? salaryUSD,
           bool isRemote,
           string description,
           string countryId);

        bool Edit(
           string id,
           bool isPaid,
           decimal? salaryUSD,
           string description);

        bool Delete(string id);

        bool Exists(string id);

        bool IsInternshipOwnedByCompany(
           string internshipId,
           string companyId);

        string GetRoleById(string id);

        InternshipDetailsViewModel GetDetailsModel(string id);

        EditInternshipFormModel GetEditModel(string id);

        InternshipListingQueryModel All(
           string role,
           bool isPaid,
           bool isRemote,
           string companyId,
           string countryId,
           int currentPage,
           int internshipsPerPage);
    }
}