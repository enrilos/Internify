namespace Internify.Services.Data.Administrator
{
    using Internify.Data;
    using Models.ViewModels.Administrator;

    public class AdministratorService : IAdministratorService
    {
        private readonly InternifyDbContext data;

        public AdministratorService(InternifyDbContext data)
            => this.data = data;

        public UserCountPerRoleViewModel GetUserCountPerRole()
            => new UserCountPerRoleViewModel
            {
                Candidates = data.Candidates.Where(x => !x.IsDeleted).Count(),
                Companies = data.Companies.Where(x => !x.IsDeleted).Count(),
                Universities = data.Universities.Where(x => !x.IsDeleted).Count(),
            };
    }
}