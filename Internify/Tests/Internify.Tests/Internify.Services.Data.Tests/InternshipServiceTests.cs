namespace Internify.Tests.Data.Services
{
    using Internify.Data;
    using Internify.Data.Models;
    using Internify.Services.Data.Company;
    using Internify.Services.Data.Internship;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.Internship;
    using Models.ViewModels.Internship;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    public class InternshipServiceTests
    {
        private InternifyDbContext data;
        private IInternshipService internshipService;
        private ICompanyService companyService;

        private string internshipId;
        private string companyId;
        private string specializationId;
        private string countryId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            internshipService = new InternshipService(data);
            companyService = new CompanyService(data, null);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ShouldReturn_NullWhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = internshipService.Add(
                "gregfwef",
                "Junior Full-Stack Developer",
                true,
                200,
                false,
                "Are you up for the challenge?",
                countryId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenCompanyIsDeleted()
        {
            SeedDatabase();
            SeedCompany();

            companyService.Delete(companyId);

            var actualResult = internshipService.Add(
                companyId,
                "Junior Full-Stack Developer",
                true,
                200,
                false,
                "Are you up for the challenge?",
                countryId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenCountryIdDoesNotMatch()
        {
            SeedDatabase();
            SeedCompany();

            var actualResult = internshipService.Add(
                companyId,
                "Junior Full-Stack Developer",
                true,
                200,
                false,
                "Are you up for the challenge?",
                "fwpefwpe");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_InternshipId()
        {
            SeedDatabase();
            SeedCompany();

            var actualResult = internshipService.Add(
                companyId,
                "Junior Full-Stack Developer",
                true,
                200,
                false,
                "Are you up for the challenge?",
                countryId);

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenInternshipIdDoesNotMatch()
        {
            var actualResult = internshipService.Edit(
                "dfwe",
                true,
                350,
                "Our company provides the best benefits and further self-improvement at our expense.");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenInternshipIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Delete(internshipId);

            var actualResult = internshipService.Edit(
                internshipId,
                true,
                350,
                "Our company provides the best benefits and further self-improvement at our expense.");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldChange_ModifiedOnProperty_AndReturn_True()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Edit(
                internshipId,
                true,
                350,
                "Our company provides the best benefits and further self-improvement at our expense.");

            var actualModifiedOnProperty = internshipService.GetDetailsModel(internshipId)?.ModifiedOn;

            Assert.IsNotNull(actualModifiedOnProperty);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenInternshipIdDoesNotMatch()
        {
            var actualResult = internshipService.Delete("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenInternshipIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Delete(internshipId);

            var actualResult = internshipService.Delete(internshipId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_True()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualDelete = internshipService.Delete(internshipId);
            var actualExists = internshipService.Exists(internshipId);

            Assert.IsTrue(actualDelete);
            Assert.IsFalse(actualExists);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenInternshipIdDoesNotMatch()
        {
            var actualResult = internshipService.Exists("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenInternshipIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Delete(internshipId);

            var actualResult = internshipService.Exists(internshipId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualResult = internshipService.Exists(internshipId);

            Assert.True(actualResult);
        }

        [Test]
        public void IsInternshipOwnedByCompany_ShouldReturn_FalseWhenInternshipIdDoesNotMatch()
        {
            SeedDatabase();
            SeedCompany();

            var actualResult = internshipService.IsInternshipOwnedByCompany(
                "fwef",
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsInternshipOwnedByCompany_ShouldReturn_FalseWhenInternshipIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Delete(internshipId);

            var actualResult = internshipService.IsInternshipOwnedByCompany(
                internshipId,
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsInternshipOwnedByCompany_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualResult = internshipService.IsInternshipOwnedByCompany(
                internshipId,
                "fwefw");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsInternshipOwnedByCompany_ShouldReturn_True()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualResult = internshipService.IsInternshipOwnedByCompany(
                internshipId,
                companyId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetRoleById_ShouldReturn_NullWhenInternshipIdDoesNotMatch()
        {
            var actualResult = internshipService.GetRoleById("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetRoleById_ShouldReturn_NullWhenInternshipIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Delete(internshipId);

            var actualResult = internshipService.GetRoleById(internshipId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetRoleById_ShouldReturn_InternshipRole()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualResult = internshipService.GetRoleById(internshipId);

            Assert.That(actualResult, Is.EqualTo("Intern Full-Stack Developer"));
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenInternshipIdDoesNotMatch()
        {
            var actualResult = internshipService.GetDetailsModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenInternshipIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Delete(internshipId);

            var actualResult = internshipService.GetDetailsModel(internshipId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_InternshipDetailsViewModel()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var expectedModel = new InternshipDetailsViewModel()
            {
                Id = internshipId,
                Role = "Intern Full-Stack Developer",
                Description = "Are you up for the challenge?",
                CompanyId = companyId,
                Country = "Bulgaria",
            };

            var actualModel = internshipService.GetDetailsModel(internshipId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Role").EqualTo(expectedModel.Role)
                                   & Has.Property("Description").EqualTo(expectedModel.Description)
                                   & Has.Property("CompanyId").EqualTo(expectedModel.CompanyId)
                                   & Has.Property("Country").EqualTo(expectedModel.Country));

            Assert.IsFalse(actualModel.IsPaid);
            Assert.IsFalse(actualModel.IsRemote);
            Assert.IsNull(actualModel.CompanyImageUrl);
            Assert.IsNotNull(actualModel.CreatedOn);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenInternshipIdDoesNotMatch()
        {
            var actualResult = internshipService.GetEditModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenInternshipIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            internshipService.Delete(internshipId);

            var actualResult = internshipService.GetEditModel(internshipId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_EditInternshipFormModel()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var expectedModel = new EditInternshipFormModel()
            {
                Id = internshipId,
                Description = "Are you up for the challenge?",
            };

            var actualModel = internshipService.GetEditModel(internshipId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Description").EqualTo(expectedModel.Description));

            Assert.IsFalse(actualModel.IsPaid);
            Assert.IsNull(actualModel.SalaryUSD);
        }

        [Test]
        public void All_ShouldReturn_InternshipListingQueryModel_WithEmptyInternshipsCollection_WhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualResult = internshipService.All(
                "developer",
                false,
                false,
                "pwoefwef",
                countryId,
                1,
                6);

            Assert.IsEmpty(actualResult.Internships);
        }

        [Test]
        public void All_ShouldReturn_InternshipListingQueryModel_WithEmptyInternshipsCollection_WhenCompanyIsDeleted()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            companyService.Delete(companyId);

            var actualResult = internshipService.All(
                "developer",
                false,
                false,
                companyId,
                countryId,
                1,
                6);

            Assert.IsEmpty(actualResult.Internships);
        }

        [Test]
        public void All_ShouldReturn_InternshipListingQueryModel_WithEmptyInternshipsCollection_WhenCountryIdDoesNotMatch()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualResult = internshipService.All(
                "developer",
                false,
                false,
                companyId,
                "fwefo[kwef",
                1,
                6);

            Assert.IsEmpty(actualResult.Internships);
        }

        [Test]
        public void All_ShouldSanitizeRole_AndReturn_InternshipListingQueryModel_WithFilteredInternshipsCollection()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var actualResult = internshipService.All(
                "developer",
                false,
                false,
                companyId,
                countryId,
                1,
                6);

            Assert.That(actualResult.Internships.Count(), Is.EqualTo(1));
        }

        [Test]
        public void All_ShouldReturn_InternshipListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            SeedDatabase();
            SeedCompany();
            SeedInternship();

            var internships = new List<Internship>()
            {
                new Internship
                {
                    CompanyId = companyId,
                    Role = "Junior Front-End Developer",
                    IsPaid = false,
                    IsRemote = true,
                    Description = "Are you up for the challenge?",
                },
                new Internship
                {
                    CompanyId = companyId,
                    Role = "Senior Software Developer",
                    IsPaid = false,
                    IsRemote = false,
                    Description = "Are you up for the challenge?",
                    CountryId = countryId
                }
            };

            data.Internships.AddRange(internships);
            data.SaveChanges();

            var actualResultOne = internshipService.All(
                null,
                false,
                false,
                companyId,
                null,
                -1,
                -1);

            var actualResultTwo = internshipService.All(
                "Senior",
                false,
                false,
                companyId,
                null,
                0,
                999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.InternshipsPerPage, Is.EqualTo(6));
            Assert.That(actualResultOne.Internships.Count(), Is.EqualTo(3));

            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.InternshipsPerPage, Is.EqualTo(96));
            Assert.That(actualResultTwo.Internships.Count(), Is.EqualTo(1));
        }

        private void SeedDatabase()
        {
            // Add Specializations, Countries
            var specialization = new Specialization
            {
                Name = "Software Development"
            };

            specializationId = specialization.Id;

            var country = new Country
            {
                Name = "Bulgaria"
            };

            countryId = country.Id;

            data.Specializations.Add(specialization);
            data.Countries.Add(country);
            data.SaveChanges();
        }

        private void SeedCompany()
        {
            // Add Identity User
            var companyAppUser = new ApplicationUser
            {
                Email = "payhawk@gmail.com",
                UserName = "payhawk@gmail.com"
            };

            data.Users.Add(companyAppUser);
            data.SaveChanges();

            // Add Company
            var company = new Company
            {
                Name = "Payhawk",
                UserId = companyAppUser.Id,
                Founded = 2014,
                Description = "Providing payment services. Fintech. Cool.",
                CEO = "Hristo Borisov",
                EmployeesCount = 200,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            companyId = company.Id;

            data.Companies.Add(company);
            data.SaveChanges();
        }

        private void SeedInternship()
        {
            var internship = new Internship
            {
                CompanyId = companyId,
                Role = "Intern Full-Stack Developer",
                IsPaid = false,
                IsRemote = false,
                Description = "Are you up for the challenge?",
                CountryId = countryId
            };

            internshipId = internship.Id;

            data.Internships.Add(internship);
            data.SaveChanges();
        }
    }
}