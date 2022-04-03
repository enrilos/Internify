namespace Internify.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    using static Internify.Common.GlobalConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static string Email(this ClaimsPrincipal user)
            => user?.FindFirst(ClaimTypes.Email)?.Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
           => user.IsInRole(AdministratorRoleName);
    }
}