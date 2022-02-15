namespace Internify.Web.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static Internify.Common.AppConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            // Seed required data...
            // TODO: Seed other users (with different roles) as well.
            SeedRoles(services);
            SeedAdministrator(services);
            SeedSpecializations(services);
            SeedCountries(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<InternifyDbContext>();

            // Temporary.
            data.Database.EnsureDeleted();

            data.Database.Migrate();
        }

        private static void SeedRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var roleNames = new string[4] { AdministratorRoleName, CandidateRoleName, CompanyRoleName, UniversityRoleName };

            Task.Run(async () =>
            {
                foreach (var role in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);

                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            })
            .GetAwaiter()
            .GetResult();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            Task
                .Run(async () =>
                {
                    const string adminEmail = "admin@admin.com";
                    const string adminPassword = "admin";

                    var user = new ApplicationUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                    };

                    // Create User
                    await userManager.CreateAsync(user, adminPassword);

                    // Assign Role To That User
                    await userManager.AddToRoleAsync(user, AdministratorRoleName);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedSpecializations(IServiceProvider services)
        {
            var specializations = new List<Specialization>() {
                new Specialization { Name = "IT, Engineering, Technology"},
                new Specialization { Name = "Marketing, Advertising, PR"},
                new Specialization { Name = "Trade and Sales"},
                new Specialization { Name = "Management"},
                new Specialization { Name = "Human Resources (HR)"},
                new Specialization { Name = "Production"},
                new Specialization { Name = "Banking, Lending, Insurance"},
                new Specialization { Name = "Administrative, Office and Business Activities"},
                new Specialization { Name = "Restaurants, Hotels, Tourism"},
                new Specialization { Name = "Drivers, Couriers"},
                new Specialization { Name = "Architecture, Construction"},
                new Specialization { Name = "Customer service centers and business services"},
                new Specialization { Name = "Accounting, Audit, Finance"},
                new Specialization { Name = "Health and Pharmacy"},
                new Specialization { Name = "Logistics, Forwarding"},
                new Specialization { Name = "Repair and Installation Activities"},
                new Specialization { Name = "Physical / Manual labor"},
                new Specialization { Name = "Cars, Auto Repairs, Gas Stations"},
                new Specialization { Name = "Entertainment, Promotions, Sports, Beauty Salons"},
                new Specialization { Name = "Cleaning, Household Services"},
                new Specialization { Name = "Telecoms"},
                new Specialization { Name = "Education, Courses, Translations"},
                new Specialization { Name = "Energy, Plumbing, Utilities"},
                new Specialization { Name = "Research and Development"},
                new Specialization { Name = "Security and Protection"},
                new Specialization { Name = "Design, Creative, Art"},
                new Specialization { Name = "Agriculture, Horticulture and Forestry"},
                new Specialization { Name = "Real Estate"},
                new Specialization { Name = "State Administration, Institutions"},
                new Specialization { Name = "Law, Legal Services"},
                new Specialization { Name = "Media, Publishing House"},
                new Specialization { Name = "Aviation, Airports and Airlines"},
                new Specialization { Name = "Non-profit organizations"},
                new Specialization { Name = "Sea and River Transport"},
            };

            var data = services.GetService<InternifyDbContext>();

            data.Specializations.AddRange(specializations);

            data.SaveChanges();
        }

        private static void SeedCountries(IServiceProvider services)
        {
            var countries = new List<Country>() {
                new Country { Name = "Afghanistan" },
                new Country { Name = "Albania" },
                new Country { Name = "Algeria" },
                new Country { Name = "Andorra" },
                new Country { Name = "Angola" },
                new Country { Name = "AntiguaandBarbuda" },
                new Country { Name = "Argentina" },
                new Country { Name = "Armenia" },
                new Country { Name = "Australia" },
                new Country { Name = "Austria" },
                new Country { Name = "Azerbaijan" },
                new Country { Name = "TheBahamas" },
                new Country { Name = "Bahrain" },
                new Country { Name = "Bangladesh" },
                new Country { Name = "Barbados" },
                new Country { Name = "Belarus" },
                new Country { Name = "Belgium" },
                new Country { Name = "Belize" },
                new Country { Name = "Benin" },
                new Country { Name = "Bhutan" },
                new Country { Name = "Bolivia" },
                new Country { Name = "BosniaandHerzegovina" },
                new Country { Name = "Botswana" },
                new Country { Name = "Brazil" },
                new Country { Name = "Brunei" },
                new Country { Name = "Bulgaria" },
                new Country { Name = "BurkinaFaso" },
                new Country { Name = "Burundi" },
                new Country { Name = "CaboVerde" },
                new Country { Name = "Cambodia" },
                new Country { Name = "Cameroon" },
                new Country { Name = "Canada" },
                new Country { Name = "CentralAfricanRepublic" },
                new Country { Name = "Chad" },
                new Country { Name = "Chile" },
                new Country { Name = "China" },
                new Country { Name = "Colombia" },
                new Country { Name = "Comoros" },
                new Country { Name = "Congo,DemocraticRepublicofthe" },
                new Country { Name = "Congo,Republicofthe" },
                new Country { Name = "CostaRica" },
                new Country { Name = "Côted’Ivoire" },
                new Country { Name = "Croatia" },
                new Country { Name = "Cuba" },
                new Country { Name = "Cyprus" },
                new Country { Name = "CzechRepublic" },
                new Country { Name = "Denmark" },
                new Country { Name = "Djibouti" },
                new Country { Name = "Dominica" },
                new Country { Name = "DominicanRepublic" },
                new Country { Name = "EastTimor" },
                new Country { Name = "Ecuador" },
                new Country { Name = "Egypt" },
                new Country { Name = "ElSalvador" },
                new Country { Name = "EquatorialGuinea" },
                new Country { Name = "Eritrea" },
                new Country { Name = "Estonia" },
                new Country { Name = "Eswatini" },
                new Country { Name = "Ethiopia" },
                new Country { Name = "Fiji" },
                new Country { Name = "Finland" },
                new Country { Name = "France" },
                new Country { Name = "Gabon" },
                new Country { Name = "TheGambia" },
                new Country { Name = "Georgia" },
                new Country { Name = "Germany" },
                new Country { Name = "Ghana" },
                new Country { Name = "Greece" },
                new Country { Name = "Grenada" },
                new Country { Name = "Guatemala" },
                new Country { Name = "Guinea" },
                new Country { Name = "Guinea-Bissau" },
                new Country { Name = "Guyana" },
                new Country { Name = "Haiti" },
                new Country { Name = "Honduras" },
                new Country { Name = "Hungary" },
                new Country { Name = "Iceland" },
                new Country { Name = "India" },
                new Country { Name = "Indonesia" },
                new Country { Name = "Iran" },
                new Country { Name = "Iraq" },
                new Country { Name = "Ireland" },
                new Country { Name = "Israel" },
                new Country { Name = "Italy" },
                new Country { Name = "Jamaica" },
                new Country { Name = "Japan" },
                new Country { Name = "Jordan" },
                new Country { Name = "Kazakhstan" },
                new Country { Name = "Kenya" },
                new Country { Name = "Kiribati" },
                new Country { Name = "Korea,North" },
                new Country { Name = "Korea,South" },
                new Country { Name = "Kosovo" },
                new Country { Name = "Kuwait" },
                new Country { Name = "Kyrgyzstan" },
                new Country { Name = "Laos" },
                new Country { Name = "Latvia" },
                new Country { Name = "Lebanon" },
                new Country { Name = "Lesotho" },
                new Country { Name = "Liberia" },
                new Country { Name = "Libya" },
                new Country { Name = "Liechtenstein" },
                new Country { Name = "Lithuania" },
                new Country { Name = "Luxembourg" },
                new Country { Name = "Madagascar" },
                new Country { Name = "Malawi" },
                new Country { Name = "Malaysia" },
                new Country { Name = "Maldives" },
                new Country { Name = "Mali" },
                new Country { Name = "Malta" },
                new Country { Name = "MarshallIslands" },
                new Country { Name = "Mauritania" },
                new Country { Name = "Mauritius" },
                new Country { Name = "Mexico" },
                new Country { Name = "Micronesia,FederatedStatesof" },
                new Country { Name = "Moldova" },
                new Country { Name = "Monaco" },
                new Country { Name = "Mongolia" },
                new Country { Name = "Montenegro" },
                new Country { Name = "Morocco" },
                new Country { Name = "Mozambique" },
                new Country { Name = "Myanmar" },
                new Country { Name = "Namibia" },
                new Country { Name = "Nauru" },
                new Country { Name = "Nepal" },
                new Country { Name = "Netherlands" },
                new Country { Name = "NewZealand" },
                new Country { Name = "Nicaragua" },
                new Country { Name = "Niger" },
                new Country { Name = "Nigeria" },
                new Country { Name = "NorthMacedonia" },
                new Country { Name = "Norway" },
                new Country { Name = "Oman" },
                new Country { Name = "Pakistan" },
                new Country { Name = "Palau" },
                new Country { Name = "Panama" },
                new Country { Name = "PapuaNewGuinea" },
                new Country { Name = "Paraguay" },
                new Country { Name = "Peru" },
                new Country { Name = "Philippines" },
                new Country { Name = "Poland" },
                new Country { Name = "Portugal" },
                new Country { Name = "Qatar" },
                new Country { Name = "Romania" },
                new Country { Name = "Russia" },
                new Country { Name = "Rwanda" },
                new Country { Name = "SaintKittsandNevis" },
                new Country { Name = "SaintLucia" },
                new Country { Name = "SaintVincentandtheGrenadines" },
                new Country { Name = "Samoa" },
                new Country { Name = "SanMarino" },
                new Country { Name = "SaoTomeandPrincipe" },
                new Country { Name = "SaudiArabia" },
                new Country { Name = "Senegal" },
                new Country { Name = "Serbia" },
                new Country { Name = "Seychelles" },
                new Country { Name = "SierraLeone" },
                new Country { Name = "Singapore" },
                new Country { Name = "Slovakia" },
                new Country { Name = "Slovenia" },
                new Country { Name = "SolomonIslands" },
                new Country { Name = "Somalia" },
                new Country { Name = "SouthAfrica" },
                new Country { Name = "Spain" },
                new Country { Name = "SriLanka" },
                new Country { Name = "Sudan" },
                new Country { Name = "Sudan,South" },
                new Country { Name = "Suriname" },
                new Country { Name = "Sweden" },
                new Country { Name = "Switzerland" },
                new Country { Name = "Syria" },
                new Country { Name = "Taiwan" },
                new Country { Name = "Tajikistan" },
                new Country { Name = "Tanzania" },
                new Country { Name = "Thailand" },
                new Country { Name = "Togo" },
                new Country { Name = "Tonga" },
                new Country { Name = "TrinidadandTobago" },
                new Country { Name = "Tunisia" },
                new Country { Name = "Turkey" },
                new Country { Name = "Turkmenistan" },
                new Country { Name = "Tuvalu" },
                new Country { Name = "Uganda" },
                new Country { Name = "Ukraine" },
                new Country { Name = "UnitedArabEmirates" },
                new Country { Name = "UnitedKingdom" },
                new Country { Name = "UnitedStates" },
                new Country { Name = "Uruguay" },
                new Country { Name = "Uzbekistan" },
                new Country { Name = "Vanuatu" },
                new Country { Name = "VaticanCity" },
                new Country { Name = "Venezuela" },
                new Country { Name = "Vietnam" },
                new Country { Name = "Yemen" },
                new Country { Name = "Zambia" },
                new Country { Name = "Zimbabwe" }
            };

            var data = services.GetService<InternifyDbContext>();

            data.Countries.AddRange(countries);

            data.SaveChanges();
        }
    }
}