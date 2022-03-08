namespace Internify.Web.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static Internify.Common.GlobalConstants;

    public static class ApplicationBuilderExtensions
    {
        private static int UsersCreatedByRole = 8;

        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedSpecializations(services);
            SeedCountries(services);
            SeedRoles(services);
            SeedAdministrator(services);
            SeedUsers(services);
            SeedCandidates(services);
            //SeedUniversities(services);
            //SeedCompanies(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<InternifyDbContext>();

            // Temporary.
            data.Database.EnsureDeleted();

            data.Database.Migrate();
        }

        private static void SeedSpecializations(IServiceProvider services)
        {
            var data = services.GetService<InternifyDbContext>();

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

            data.Specializations.AddRange(specializations);

            data.SaveChanges();
        }

        private static void SeedCountries(IServiceProvider services)
        {
            var data = services.GetService<InternifyDbContext>();

            var countries = new List<Country>() {
                new Country { Name = "Afghanistan" },
                new Country { Name = "Albania" },
                new Country { Name = "Algeria" },
                new Country { Name = "Andorra" },
                new Country { Name = "Angola" },
                new Country { Name = "Antigua and Barbuda" },
                new Country { Name = "Argentina" },
                new Country { Name = "Armenia" },
                new Country { Name = "Australia" },
                new Country { Name = "Austria" },
                new Country { Name = "Azerbaijan" },
                new Country { Name = "The Bahamas" },
                new Country { Name = "Bahrain" },
                new Country { Name = "Bangladesh" },
                new Country { Name = "Barbados" },
                new Country { Name = "Belarus" },
                new Country { Name = "Belgium" },
                new Country { Name = "Belize" },
                new Country { Name = "Benin" },
                new Country { Name = "Bhutan" },
                new Country { Name = "Bolivia" },
                new Country { Name = "Bosnia and Herzegovina" },
                new Country { Name = "Botswana" },
                new Country { Name = "Brazil" },
                new Country { Name = "Brunei" },
                new Country { Name = "Bulgaria" },
                new Country { Name = "Burkina Faso" },
                new Country { Name = "Burundi" },
                new Country { Name = "Cabo Verde" },
                new Country { Name = "Cambodia" },
                new Country { Name = "Cameroon" },
                new Country { Name = "Canada" },
                new Country { Name = "Central African Republic" },
                new Country { Name = "Chad" },
                new Country { Name = "Chile" },
                new Country { Name = "China" },
                new Country { Name = "Colombia" },
                new Country { Name = "Comoros" },
                new Country { Name = "Democratic Republic of the Congo" },
                new Country { Name = "Costa Rica" },
                new Country { Name = "Côte d’Ivoire" },
                new Country { Name = "Croatia" },
                new Country { Name = "Cuba" },
                new Country { Name = "Cyprus" },
                new Country { Name = "Czech Republic" },
                new Country { Name = "Denmark" },
                new Country { Name = "Djibouti" },
                new Country { Name = "Dominica" },
                new Country { Name = "Dominican Republic" },
                new Country { Name = "East Timor" },
                new Country { Name = "Ecuador" },
                new Country { Name = "Egypt" },
                new Country { Name = "El Salvador" },
                new Country { Name = "Equatorial Guinea" },
                new Country { Name = "Eritrea" },
                new Country { Name = "Estonia" },
                new Country { Name = "Eswatini" },
                new Country { Name = "Ethiopia" },
                new Country { Name = "Fiji" },
                new Country { Name = "Finland" },
                new Country { Name = "France" },
                new Country { Name = "Gabon" },
                new Country { Name = "The Gambia" },
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
                new Country { Name = "North Korea" },
                new Country { Name = "South Korea" },
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
                new Country { Name = "Marshall Islands" },
                new Country { Name = "Mauritania" },
                new Country { Name = "Mauritius" },
                new Country { Name = "Mexico" },
                new Country { Name = "Federated States of Micronesia" },
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
                new Country { Name = "New Zealand" },
                new Country { Name = "Nicaragua" },
                new Country { Name = "Niger" },
                new Country { Name = "Nigeria" },
                new Country { Name = "North Macedonia" },
                new Country { Name = "Norway" },
                new Country { Name = "Oman" },
                new Country { Name = "Pakistan" },
                new Country { Name = "Palau" },
                new Country { Name = "Panama" },
                new Country { Name = "Papua New Guinea" },
                new Country { Name = "Paraguay" },
                new Country { Name = "Peru" },
                new Country { Name = "Philippines" },
                new Country { Name = "Poland" },
                new Country { Name = "Portugal" },
                new Country { Name = "Qatar" },
                new Country { Name = "Romania" },
                new Country { Name = "Russia" },
                new Country { Name = "Rwanda" },
                new Country { Name = "Saint Kitts and Nevis" },
                new Country { Name = "Saint Lucia" },
                new Country { Name = "Saint Vincent and the Grenadines" },
                new Country { Name = "Samoa" },
                new Country { Name = "San Marino" },
                new Country { Name = "São Tomé and Príncipe" },
                new Country { Name = "Saudi Arabia" },
                new Country { Name = "Senegal" },
                new Country { Name = "Serbia" },
                new Country { Name = "Seychelles" },
                new Country { Name = "Sierra Leone" },
                new Country { Name = "Singapore" },
                new Country { Name = "Slovakia" },
                new Country { Name = "Slovenia" },
                new Country { Name = "Solomon Islands" },
                new Country { Name = "Somalia" },
                new Country { Name = "South Africa" },
                new Country { Name = "Spain" },
                new Country { Name = "Sri Lanka" },
                new Country { Name = "Sudan" },
                new Country { Name = "South Sudan" },
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
                new Country { Name = "Trinidad and Tobago" },
                new Country { Name = "Tunisia" },
                new Country { Name = "Turkey" },
                new Country { Name = "Turkmenistan" },
                new Country { Name = "Tuvalu" },
                new Country { Name = "Uganda" },
                new Country { Name = "Ukraine" },
                new Country { Name = "United Arab Emirates" },
                new Country { Name = "United Kingdom" },
                new Country { Name = "United States" },
                new Country { Name = "Uruguay" },
                new Country { Name = "Uzbekistan" },
                new Country { Name = "Vanuatu" },
                new Country { Name = "Vatican City" },
                new Country { Name = "Venezuela" },
                new Country { Name = "Vietnam" },
                new Country { Name = "Yemen" },
                new Country { Name = "Zambia" },
                new Country { Name = "Zimbabwe" }
            };

            data.Countries.AddRange(countries);

            data.SaveChanges();
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
                    const string adminEmail = "admin@gmail.com";
                    const string adminPassword = "admin123";

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

        private static void SeedUsers(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Email = "nakov@gmail.com",
                    UserName = "nakov@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "kenov@gmail.com",
                    UserName = "kenov@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "kostov@gmail.com",
                    UserName = "kostov@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "papazov@gmail.com",
                    UserName = "papazov@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "markish@gmail.com",
                    UserName = "markish@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "vasilev@gmail.com",
                    UserName = "vasilev@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "emre@gmail.com",
                    UserName = "emre@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "lock@gmail.com",
                    UserName = "lock@gmail.com"
                },
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // university
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
                //new ApplicationUser
                //{
                //    // company
                //},
            };

            var candidates = users.Take(UsersCreatedByRole);

            foreach (var candidate in candidates)
            {
                Task.Run(async () =>
                {
                    var result = await userManager.CreateAsync(candidate, $"{candidate.Email.Split('@')[0]}123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(candidate, CandidateRoleName);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unable to assign {CandidateRoleName} role to {candidate.Email}");
                    }
                })
                .GetAwaiter()
                .GetResult();
            }

            var universities = users
                .Skip(UsersCreatedByRole)
                .Take(UsersCreatedByRole);

            foreach (var university in universities)
            {
                Task.Run(async () =>
                {
                    var result = await userManager.CreateAsync(university, $"{university.Email.Split('@')[0]}123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(university, UniversityRoleName);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unable to assign {UniversityRoleName} role to {university.Email}");
                    }
                })
                .GetAwaiter()
                .GetResult();
            }

            var companies = users.Skip(UsersCreatedByRole * 2);

            foreach (var company in companies)
            {
                Task.Run(async () =>
                {
                    var result = await userManager.CreateAsync(company, $"{company.Email.Split('@')[0]}123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(company, CompanyRoleName);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unable to assign {CompanyRoleName} role to {company.Email}");
                    }
                })
                .GetAwaiter()
                .GetResult();
            }
        }

        private static void SeedCandidates(IServiceProvider services)
        {
            var data = services.GetService<InternifyDbContext>();
            var users = data.Users.ToList();

            var candidates = new List<Candidate>()
            {
                new Candidate
                {
                    FirstName = "Svetlin",
                    LastName = "Nakov",
                    ImageUrl = "https://avatars.githubusercontent.com/u/1689586?v=4",
                    WebsiteUrl = "nakov.com",
                    Description = @"Svetlin Nakov has 20+ years of technical background as software engineer, software project manager, consultant, trainer and entrepreneur with rich experience with Web development, information systems, databases, blockchain, cryptography, .NET, Java EE, JavaScript, PHP, Python and software engineering. He is the leading author of 15 books on computer programming, software technologies, cryptography, C#, Java, JavaScript, Python and tens of technical and scientific publications. He is a big fan of knowledge sharing and is proud Wikipedia contributor, free books author and open-source supporter.

Svetlin has been a speaker at hundreds of conferences, seminars, meetups, courses and other trainings in the United States, Singapore, Germany, Egypt, Bulgaria and other locations. He holds a PhD degree in computer science (for his research on computational linguistics and machine learning), several medals from the International Informatics Olympiads (IOI) and the Bulgarian President’s award “John Atanasoff”. He has been a part-time assistant professor / visiting speaker / technical trainer in Sofia University, New Bulgarian University, the Technical University of Sofia, Ngee Ann Polytechnic (Singapore) and few others.

In the last few years Svetlin Nakov together with his partners drive the global expansion of the largest training center for software engineers in Bulgaria and the region – the Software University, where he inspires and teaches hundred of thousands of young people in computer science, software development, information technologies and digital skills, and gives them a profession and a job.

Public Profiles
YouTube: https://youtube.com/c/CodeWithNakov

LinkedIn: https://linkedin.com/in/nakov

GitHub: https://github.com/nakov",
                    BirthDate = DateTime.Parse("05/22/1980 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("nakov")).Id
                },
                new Candidate
                {
                    FirstName = "Ivaylo",
                    LastName = "Kenov",
                    ImageUrl = "https://avatars.githubusercontent.com/u/3391906?v=4",
                    WebsiteUrl = "codelessons.online",
                    Description = @"Ivaylo Kenov is a Software Engineer and Technical Trainer, born in 1989, and based in Sofia, Bulgaria. According to the latest Git Awards Ivaylo ranks worldwide C# Developer #85 and #2 in Bulgaria.

Ivaylo showed strong interest in Mathematics and Logic at an early age. He was among the Top 5 participants in more than 10 different Maths Olympics. It only came naturally for him to choose the High School of Maths and Science in his hometown, Kyustendil.

In 2013 Ivaylo graduated from the University of Architecture, Civil Engineering, and Geodesy. By that time, he already knew he wasn't keen on construction, but was passionate about solving logical problems in his day-to-day professional life.

The same year Kenov got enrolled with the Telerik Academy and got on to rule in their courses – he became Top Champion in both Object-Oriented Programming, and Data Structures and Algorithms.

He currently is:

The CTO of a large Software-Engineering academy
Author to numerous open-source projects
Bearer of more than 3000 likes across his nearly 60 repos
Technical trainer to more than 5000 on-site and uncountable online students
Ivaylo Kenov's Fluent Testing Library – My Tested ASP.NET – has been featured several times in Microsoft's Developer Blogs and Repositories, in their official ASP.NET Core MVC repository.

Microsoft also happen to recommend Kenov's lectures within their official documentation.

Ivaylo Kenov was born on April 9, 1989.",
                    BirthDate = DateTime.Parse("04/09/1989 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("kenov")).Id
                },
                new Candidate
                {
                    FirstName = "Nikolay",
                    LastName = "Kostov",
                    ImageUrl = "https://avatars.githubusercontent.com/u/3106986?v=4",
                    WebsiteUrl = "nikolay.it",
                    Description = @"Nikolay is a very experienced software engineer with architectural skills and 10+ years in full-stack web development—specializing at ASP.NET MVC. He possesses a deep understanding of algorithmic problems and machine learning. He works with precision and is keen on keeping deadlines. Nikolay is also a Microsoft Certified Trainer (MCT) with more than 15 Microsoft certificates.",
                    BirthDate = DateTime.Parse("06/20/1990 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("kostov")).Id
                },
                new Candidate
                {
                    FirstName = "Ivaylo",
                    LastName = "Papazov",
                    ImageUrl = "https://avatars.githubusercontent.com/u/6164175?v=4",
                    WebsiteUrl = null,
                    Description = null,
                    BirthDate = DateTime.Parse("10/10/1988 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("papazov")).Id
                },
                new Candidate
                {
                    FirstName = "Prakash",
                    LastName = "Markish",
                    ImageUrl = "https://im.indiatimes.in/content/2021/Nov/software-engineer_619f38104827c.jpg",
                    WebsiteUrl = null,
                    Description = "I believe in code and the fact that everything has it logical explanation.",
                    BirthDate = DateTime.Parse("10/22/1997 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "India"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("markish")).Id
                },
                new Candidate
                {
                    FirstName = "Atanas",
                    LastName = "Vasilev",
                    ImageUrl = "https://avatars.githubusercontent.com/u/38570429?v=4",
                    WebsiteUrl = "https://naskovasilev.github.io/CV/",
                    Description = @"My name is Atanas Vasilev and I am born in 24/11/2001. Currently I am student in the Faculty of Mathematics and Informatics in Sofia.

My favourite programming language is C#, but I also like programming in JavaScript.

I am sociable and reliable person. I like learning new things every day. I like helping others and share my knowledge with them about everything that interests me about technologies.",
                    BirthDate = DateTime.Parse("11/24/2001 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("vasilev")).Id
                },
                new Candidate
                {
                    FirstName = "Emre",
                    LastName = "Ereceb",
                    ImageUrl = "https://avatars.githubusercontent.com/u/51094983?v=4",
                    WebsiteUrl = "https://github.com/enrilos",
                    Description = @"I am an ambitious, devoted and open-minded individual who has a tendency to adhere to deadlines as well as being incredibly inquisitive at learning new technologies with dedication. Furthermore, I can confidently proclaim the fact that I am remarkably stress-resistant.",
                    BirthDate = DateTime.Parse("12/19/2001 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("emre")).Id
                },
                new Candidate
                {
                    FirstName = "Andrew",
                    LastName = "Lock",
                    ImageUrl = "https://avatars.githubusercontent.com/u/18755388?v=4",
                    WebsiteUrl = "https://andrewlock.net/",
                    Description = @"My name is Andrew Lock, though everyone knows me as ‘Sock’. I am a full-time developer, working predominantly in full stack ASP.NET development in Devon, UK. I graduated with an MEng in Engineering from Cambridge University in 2008, and completed my PhD in Medical Image Processing in 2014. I have experience primarily with C# and VB ASP.NET, working both in MVC and WebForms, but have also worked professionally with C++ and WinForms.",
                    BirthDate = DateTime.Parse("08/08/1986 12:00:00 AM"),
                    Gender = Gender.Male,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "United Kingdom"),
                    UserId = users.FirstOrDefault(x => x.Email.ToLower().Contains("lock")).Id
                },
            };

            data.Candidates.AddRange(candidates);

            data.SaveChanges();
        }

        private static void SeedUniversities(IServiceProvider services)
        {
            var data = services.GetService<InternifyDbContext>();
            var users = data.Users
                .Skip(UsersCreatedByRole)
                .Take(UsersCreatedByRole)
                .ToList();

            var universities = new List<University>()
            {
                new University
                {
                    UserId = users[0].Id
                },
                new University
                {
                    UserId = users[1].Id
                },
                new University
                {
                    UserId = users[2].Id
                },
                new University
                {
                    UserId = users[3].Id
                },
                new University
                {
                    UserId = users[4].Id
                },
                new University
                {
                    UserId = users[5].Id
                },
                new University
                {
                    UserId = users[6].Id
                },
                new University
                {
                    UserId = users[7].Id
                },
            };

            data.Universities.AddRange(universities);

            data.SaveChanges();
        }

        private static void SeedCompanies(IServiceProvider services)
        {
            var data = services.GetService<InternifyDbContext>();
            var users = data.Companies
                .Skip(UsersCreatedByRole * 2)
                .ToList();

            var companies = new List<Company>()
            {
                new Company
                {
                    UserId = users[0].Id
                },
                new Company
                {
                    UserId = users[1].Id
                },
                new Company
                {
                    UserId = users[2].Id
                },
                new Company
                {
                    UserId = users[3].Id
                },
                new Company
                {
                    UserId = users[4].Id
                },
                new Company
                {
                    UserId = users[5].Id
                },
                new Company
                {
                    UserId = users[6].Id
                },
                new Company
                {
                    UserId = users[7].Id
                },
            };

            data.Companies.AddRange(companies);

            data.SaveChanges();
        }

        private static string GetSpecializationIdByName(InternifyDbContext data, string name)
            => data
            .Specializations
            .FirstOrDefault(x => x.Name.ToLower() == name.ToLower())?.Id;

        private static string GetCountryIdByName(InternifyDbContext data, string name)
            => data
            .Countries
            .FirstOrDefault(x => x.Name.ToLower() == name.ToLower())?.Id;
    }
}