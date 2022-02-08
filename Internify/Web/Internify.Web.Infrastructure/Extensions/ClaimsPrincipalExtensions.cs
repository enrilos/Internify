namespace Internify.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    using static Common.WebConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
           => user.IsInRole(AdministratorRoleName);

        public static bool IsCandidate(this ClaimsPrincipal user)
            => user.IsInRole(CandidateRoleName);

        public static bool IsCompany(this ClaimsPrincipal user)
            => user.IsInRole(CompanyRoleName);

        public static bool IsUniversity(this ClaimsPrincipal user)
            => user.IsInRole(UniversityRoleName);
    }
}
