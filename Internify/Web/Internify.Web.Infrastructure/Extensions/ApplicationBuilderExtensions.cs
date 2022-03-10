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

            //SeedSpecializations(services);
            //SeedCountries(services);
            //SeedRoles(services);
            //SeedAdministrator(services);
            //SeedUsers(services);
            //SeedCandidates(services);
            //SeedUniversities(services);
            //SeedCompanies(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<InternifyDbContext>();

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
                new ApplicationUser
                {
                    Email = "nbu@gmail.com",
                    UserName = "nbu@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "mit@gmail.com",
                    UserName = "mit@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "babson@gmail.com",
                    UserName = "babson@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "telaviv@gmail.com",
                    UserName = "telaviv@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "stanford@gmail.com",
                    UserName = "stanford@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "yale@gmail.com",
                    UserName = "yale@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "softuni@gmail.com",
                    UserName = "softuni@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "paris@gmail.com",
                    UserName = "paris@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "novoresume@gmail.com",
                    UserName = "novoresume@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "postbank@gmail.com",
                    UserName = "postbank@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "smartit@gmail.com",
                    UserName = "smartit@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "americaneagle@gmail.com",
                    UserName = "americaneagle@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "lidl@gmail.com",
                    UserName = "lidl@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "codexio@gmail.com",
                    UserName = "codexio@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "motion@gmail.com",
                    UserName = "motion@gmail.com"
                },
                new ApplicationUser
                {
                    Email = "cocacola@gmail.com",
                    UserName = "cocacola@gmail.com"
                },
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
                    Description = "I believe in code and the fact that everything has its logical explanation.",
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
                    Description = @"My name is Atanas Vasilev and I am born in 24/11/2001. Currently, I am student in the Faculty of Mathematics and Informatics in Sofia.

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
            var users = data.Users.ToList();

            var universities = new List<University>()
            {
                new University
                {
                    Name = "New Bulgarian University",
                    ImageUrl = "https://www.world-education.eu/uploads/58c29b8823c3425541214124c02e90e175a2f9e1.png",
                    WebsiteUrl = "nbu.bg",
                    Founded = 1991,
                    Type = Type.Private,
                    Description = @"New Bulgarian University is a private university based in Sofia, the capital of Bulgaria. Its campus is in the western district of the city, known for its proximity to the Vitosha nature park. The university also owns multiple other buildings across the country, as well as its own publishing house and a library.",
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("nbu")).Id
                },
                new University
                {
                    Name = "Massachusetts Institute of Technology",
                    ImageUrl = "https://www.newtondesk.com/wp-content/uploads/2018/05/MIT-Massachusetts-Institute-of-Technology.jpg",
                    WebsiteUrl = "https://www.mit.edu/",
                    Founded = 1861,
                    Type = Type.Private,
                    Description = @"The Massachusetts Institute of Technology (MIT) is a private land-grant research university in Cambridge, Massachusetts. Established in 1861, MIT has since played a key role in the development of modern technology and science, ranking it among the top academic institutions in the world.

Founded in response to the increasing industrialization of the United States, MIT adopted a European polytechnic university model and stressed laboratory instruction in applied science and engineering. The institute has an urban campus that extends more than a mile (1.6 km) alongside the Charles River, and encompasses a number of major off-campus facilities such as the MIT Lincoln Laboratory, the Bates Center, and the Haystack Observatory, as well as affiliated laboratories such as the Broad and Whitehead Institutes.",
                    CountryId = GetCountryIdByName(data, "United States"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("mit")).Id
                },
                new University
                {
                    Name = "Babson College",
                    ImageUrl = "https://meaningful.business/wp-content/uploads/2021/02/babson-500x500.png",
                    WebsiteUrl = "babson.edu",
                    Founded = 1919,
                    Type = Type.Private,
                    Description = @"Babson College is a private business school in Wellesley, Massachusetts. Established in 1919, its central focus is on entrepreneurship education. It was founded by Roger W. Babson as an all-male business institute, but became coeducational in 1970.",
                    CountryId = GetCountryIdByName(data, "United States"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("babson")).Id
                },
                new University
                {
                    Name = "Tel Aviv University",
                    ImageUrl = "https://satchifainarolab.com/wp-content/uploads/2019/09/telaviv-e1507849017494.png",
                    WebsiteUrl = "https://english.tau.ac.il/",
                    Founded = 1956,
                    Type = Type.Public,
                    Description = @"Tel Aviv University is a public research university in Tel Aviv, Israel. With over 30,000 students, it is the largest university in the country. Located in northwest Tel Aviv, the university is the center of teaching and research of the city, comprising 9 faculties, 17 teaching hospitals, 18 performing arts centers, 27 schools, 106 departments, 340 research centers, and 400 laboratories.

Besides being the largest university in Israel, Tel Aviv University is also the largest Jewish university in the world. It originated in 1956 when three education units merged to form the university. The original 170-acre campus was expanded and now makes up 220 acres (89 hectares) in Tel Aviv's Ramat Aviv neighborhood.",
                    CountryId = GetCountryIdByName(data, "Israel"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("telaviv")).Id
                },
                new University
                {
                    Name = "Stanford University",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/b/b7/Stanford_University_seal_2003.svg/1200px-Stanford_University_seal_2003.svg.png",
                    WebsiteUrl = "www.stanford.edu",
                    Founded = 1885,
                    Type = Type.Private,
                    Description = @"Stanford University, officially Leland Stanford Junior University, is a private research university located in the census-designated place of Stanford, California, near the city of Palo Alto. The campus occupies 8,180 acres (3,310 hectares), among the largest in the United States, and enrolls over 17,000 students. Stanford is ranked among the top universities in the world.
The university is organized around seven schools: three schools consisting of 40 academic departments at the undergraduate level as well as four professional schools that focus on graduate programs in law, medicine, education, and business. All schools are on the same campus. Students compete in 36 varsity sports, and the university is one of two private institutions in the Division I FBS Pac-12 Conference. Stanford has won 137 NCAA team championships, more than any other university, and was awarded the NACDA Directors' Cup for 25 consecutive years, beginning in 1994–1995. In addition, by 2021, Stanford students and alumni had won at least 296 Olympic medals including 150 gold medals.",
                    CountryId = GetCountryIdByName(data, "United States"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("stanford")).Id
                },
                new University
                {
                    Name = "Yale University",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/07/Yale_University_Shield_1.svg/419px-Yale_University_Shield_1.svg.png",
                    WebsiteUrl = "https://www.yale.edu/",
                    Founded = 1701,
                    Type = Type.Private,
                    Description = @"Yale University is a private Ivy League research university in New Haven, Connecticut. Founded in 1701 as the Collegiate School, it is the third-oldest institution of higher education in the United States and among the most prestigious in the world.

Chartered by Connecticut Colony, the Collegiate School was established in 1701 by clergy to educate Congregational ministers before moving to New Haven in 1716. Originally restricted to theology and sacred languages, the curriculum began to incorporate humanities and sciences by the time of the American Revolution. In the 19th century, the college expanded into graduate and professional instruction, awarding the first PhD in the United States in 1861 and organizing as a university in 1887. Yale's faculty and student populations grew after 1890 with rapid expansion of the physical campus and scientific research.",
                    CountryId = GetCountryIdByName(data, "United States"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("yale")).Id
                },
                new University
                {
                    Name = "Software University",
                    ImageUrl = "https://yt3.ggpht.com/ytc/AKedOLSp0TNfguM4s9wg2yTKv-_wzdu9VAZxAPnWSNYWKo4=s900-c-k-c0x00ffffff-no-rj",
                    WebsiteUrl = "softuni.bg",
                    Founded = 2013,
                    Type = Type.Private,
                    Description = @"We make the people we train real professionals in the software industry and work for their career start!
SoftUni's Software University project was founded with the idea of ​​an innovative and modern educational center that creates true professionals in the world of programming. For us, as for the entire software industry, the most important are real practical skills.

That is why we use the learning by doing model, providing our students with programming training with real practical experience and knowledge in the most popular and modern technologies, preparing to start your career as a successful software engineer.",
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("softuni")).Id
                },
                new University
                {
                    Name = "Paris School of Economics",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/bb/Logo_pse_petit.jpg",
                    WebsiteUrl = "https://www.parisschoolofeconomics.eu/en/",
                    Founded = 2006,
                    Type = Type.Public,
                    Description = @"The Paris School of Economics (PSE; French: École d'économie de Paris) is a French research institute in the field of economics. It offers MPhil, MSc, and PhD level programmes in various fields of theoretical and applied economics, including macroeconomics, econometrics, political economy and international economics.

PSE is a brainchild of the École des Hautes Études en Sciences Sociales (EHESS, where the students are enrolled primarily), the École Normale Supérieure, the École des Ponts and University of Paris 1 Pantheon-Sorbonne, and it is physically located on the ENS campus of Jourdan in the 14th arrondissement of Paris. It was founded in 2006 as a coalition of universities and grandes écoles to unify high-level research in economics across French academia, and was first presided by economist Thomas Piketty. Since its foundation it has gained a certain amount of academic weight, and according to a ranking released by project RePEc in May 2020, it was ranked as the fifth-best university-level economics department in the world and first in Europe. Paris School of Economics' ranking has consistently risen since it was listed on the rankings on RePEc.",
                    CountryId = GetCountryIdByName(data, "France"),
                    UserId = users.FirstOrDefault(x => x.Email.Contains("paris")).Id
                },
            };

            data.Universities.AddRange(universities);

            data.SaveChanges();
        }

        private static void SeedCompanies(IServiceProvider services)
        {
            var data = services.GetService<InternifyDbContext>();
            var users = data.Users.ToList();

            var companies = new List<Company>()
            {
                new Company
                {
                    Name = "Novoresume",
                    ImageUrl = "https://advancemed.com.au/wp-content/uploads/2019/11/vertical-color.png",
                    WebsiteUrl = "novoresume.com",
                    Founded = 2014,
                    Description = @"Novorésumé began in 2014 when Andrei, Cristian, and Stefan noticed a common problem among several of their contacts. Despite having extensive work experience and impressive skill sets, these individuals didn’t know how to showcase their talents with a professional resume and cover letter. Sensing an opportunity to help others with their job search, the three of them joined forces in exploring possible solutions as part of a university project.",
                    RevenueUSD = 67512587,
                    CEO = "Stefan Polexe",
                    EmployeesCount = 12,
                    IsPublic = false,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Belgium"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("novoresume")).Id
                },
                new Company
                {
                    Name = "Postbank",
                    ImageUrl = "https://www.theswiftcodes.com/images/bank-logo/bulgaria/postbank.png",
                    WebsiteUrl = "postbank.bg",
                    Founded = 1991,
                    Description = @"Postbank has a 30-year presence among the leaders in the banking market in Bulgaria. The bank is a leading factor in innovation and the formation of trends in the banking sector in the country in recent years and has been awarded many times for its innovations. Postbank occupies a strategic place in retail banking and corporate banking in Bulgaria. It is one of the leaders in the market of credit and debit cards, housing and consumer lending, savings products, as well as in terms of products for corporate clients - from small companies to large international companies with a presence in the country. The bank has one of the best developed branch networks and modern digital banking channels.

In just a few years, Postbank completed two successful transactions by acquiring and integrating in record time, first Alfa Bank - Bulgaria Branch, and then Piraeus Bank Bulgaria. They are the next step in consolidating its position as a systemic bank and expanding its customer base.

The bank is a member of Eurobank Group - a dynamic banking organization operating in six countries, with total assets of 70.9 billion euros and nearly 11,329 employees.",
                    RevenueUSD = 77999999,
                    CEO = "Petya Dimitrova",
                    EmployeesCount = 3000,
                    IsPublic = false,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "Banking, Lending, Insurance"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("postbank")).Id
                },
                new Company
                {
                    Name = "Smart IT",
                    ImageUrl = "https://digitalk.bg/shimg/zx952y526_4126656.jpg",
                    WebsiteUrl = "smartit.bg",
                    Founded = 2006,
                    Description = @"Smart IT is the technological hub that ensures the work of 8300 employees and the companies in Management Financial Group in Bulgaria, Romania, Poland, Ukraine, Northern Macedonia, and Spain. The business entities of MFG have over 16 years of experience in developing and managing complex software solutions and in administrating ICT infrastructure. 

The main focus of Smart IT is the development of a fully generic modular no-code platform for onboarding of fintech companies with various business models. We use technologies such as C# and .NET/Core, ASP.NET/Core MVC, Web API, RabbitMQ, OAuth 2.0, Microservices, Docker, Kubernetes, ELK Stack, Grafana/InfluxDB, Polymer, DevExpress, JavaScript, jQuery, SignalR, EF, EF Core, MS SQL, MongoDB.",
                    CEO = "Bogdan Radostinov",
                    EmployeesCount = 130,
                    IsPublic = true,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("smartit")).Id
                },
                new Company
                {
                    Name = "American Eagle Software, Inc.",
                    ImageUrl = "https://ameagle-assets.azureedge.net/aecom-blobs/images/default-source/news-images/americaneagle-com1261183069.png",
                    WebsiteUrl = "americaneagle.com",
                    Founded = 1978,
                    Description = @"Partnering with a family-owned, industry leader with a sole focus on helping customers grow and achieve success is a winning combination. The team at Americaneagle.com understands that each client has a different story and unique digital goals. Regardless of business size, industry, or technology, our talented team has a proven track record of delivering exciting, high-performing digital solutions that produce positive results for businesses across the globe.",
                    CEO = "Chris Pratt",
                    EmployeesCount = 650,
                    IsPublic = false,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "United States"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("americaneagle")).Id
                },
                new Company
                {
                    Name = "Lidl Bulgaria",
                    ImageUrl = "https://www.lidl.bg/bundles/retail/images/meta/og_default_600_600.png",
                    WebsiteUrl = "lidl.bg",
                    Founded = 1973,
                    Description = @"Lidl Stiftung & Co. KG is a German international discount retailer chain that operates over 11,000 stores across Europe and the United States. Headquartered in Neckarsulm, Baden-Württemberg, the company belongs to the Schwarz Group, which also operates the hypermarket chain Kaufland.

Lidl is the chief competitor of the similar German discount chain Aldi in several markets. There are Lidl stores in every member state of the European Union as well as in Switzerland, Serbia, the United Kingdom, and the United States.",
                    RevenueUSD = 57000000000,
                    CEO = "Gerd Chrzanowski",
                    EmployeesCount = 315000,
                    IsPublic = false,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "Trade and Sales"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("lidl")).Id
                },
                new Company
                {
                    Name = "Codexio",
                    ImageUrl = "https://assets.jobs.bg/assets/cover_photo/2021-06-07/s_72e820a74ccad9bf6a84c4eb084e8273.png",
                    WebsiteUrl = "codexio.bg",
                    Founded = 2017,
                    Description = @"Codexio is a fast-growing software company. We offer complex software solutions, development of internal projects, and external consulting services in the eCommerce and Fintech field. Our team includes passionate experts who have the ability to build next-generation IT solutions. Creativity, teamwork, and learning have been encouraged since its establishment in 2017.
Using modern technologies and methodologies we are constantly seeking to improve our services.",
                    CEO = "Ivan Yonkov (RoYaL)",
                    EmployeesCount = 20,
                    IsPublic = false,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("codexio")).Id
                },
                new Company
                {
                    Name = "Motion Software",
                    ImageUrl = "https://lh3.ggpht.com/p/AF1QipP-2bH32lsbcKL8AhAj2FUzAu1uC0lAA9PBHdhx=s1536",
                    WebsiteUrl = "motion-software.com",
                    Founded = 2015,
                    Description = @"Motion Software started as a small collective of software specialists truly passionate about creating an organization where everyone is encouraged to develop the skills and personal qualities required to achieve one’s life goals and make dreams come true. Today, we are grateful to be trusted by the world’s leading companies in providing top talent and critical remote work infrastructure when they need it the most.",
                    CEO = "Christo Peev",
                    EmployeesCount = 89,
                    IsPublic = false,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "IT, Engineering, Technology"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("motion")).Id
                },
                new Company
                {
                    Name = "Coca Cola Bulgaria",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/ce/Coca-Cola_logo.svg/1024px-Coca-Cola_logo.svg.png",
                    WebsiteUrl = "https://www.coca-cola.bg/",
                    Founded = 1886,
                    Description = "Does it need an introduction?",
                    CEO = "James Quincey",
                    EmployeesCount = 79000,
                    IsPublic = true,
                    IsGovernmentOwned = false,
                    SpecializationId = GetSpecializationIdByName(data, "Trade and Sales"),
                    CountryId = GetCountryIdByName(data, "Bulgaria"),
                    UserId = data.Users.FirstOrDefault(x => x.Email.Contains("cocacola")).Id
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