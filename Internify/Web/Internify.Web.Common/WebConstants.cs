namespace Internify.Web.Common
{
    using System;

    public class WebConstants
    {
        public const string AdministratorRoleName = "Administrator";
        public const string CandidateRoleName = "Candidate";
        public const string CompanyRoleName = "Company";
        public const string UniversityRoleName = "University";

        public static DateTime MinDateAllowed = DateTime.UtcNow.AddYears(-100);
        public static DateTime MaxDateAllowed = DateTime.UtcNow.AddYears(-16);

        public const string GlobalMessageKey = "GlobalMessage";

        public const string SpecializationsCacheKey = nameof(SpecializationsCacheKey);
        public const string CountriesCacheKey = nameof(CountriesCacheKey);
    }
}