namespace Internify.Services.Data.Administrator
{
    using Models.ViewModels.Administrator;

    public interface IAdministratorService
    {
        UserCountPerRoleViewModel GetUserCountPerRole();
    }
}