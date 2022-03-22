namespace Internify.Services.Administrator
{
    using Models.ViewModels.Administrator;

    public interface IAdministratorService
    {
        UserCountPerRoleViewModel GetUserCountPerRole();
    }
}