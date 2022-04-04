namespace Internify.Services.Data.Tests
{
    using Candidate;
    using Company;
    using Internify.Data;
    using Internify.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.Company;
    using Models.ViewModels.Company;
    using NUnit.Framework;
    using System;
    using System.Linq;

    public class CompanyServiceTests
    {
        private InternifyDbContext data;
        private ICompanyService companyService;
        private ICandidateService candidateService;

        private string companyId;
        private string companyAppUserId;
        private string specializationId;
        private string countryId;
        private string candidateId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            companyService = new CompanyService(data, null);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void IsCompany_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            var actualResult = companyService.IsCompany("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCompany_ShouldReturn_FalseWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.IsCompany(companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCompany_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = companyService.IsCompany(companyId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsCompanyByUserId_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            var actualResult = companyService.IsCompanyByUserId("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCompanyByUserId_ShouldReturn_FalseWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.IsCompanyByUserId(companyAppUserId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCompanyByUserId_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = companyService.IsCompanyByUserId(companyAppUserId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_NullWhenUserIdDoesNotMatch()
        {
            var actualResult = companyService.GetIdByUserId(companyAppUserId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_NullWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.GetIdByUserId(companyAppUserId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_CompanyId()
        {
            SeedDatabase();

            var actualResult = companyService.GetIdByUserId(companyAppUserId);

            Assert.That(actualResult, Is.EqualTo(companyId));
        }

        [Test]
        public void Add_ShouldReturn_NullWhenUserIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.Add(
                "fwef",
                "payhawk",
                null,
                null,
                2014,
                "Payment service. Payhawk Payhawk Fintech. Cool.",
                null,
                "Hristo Borisov",
                200,
                false,
                false,
                specializationId,
                countryId,
                "somecooldomain.com");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenSpecializationIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.Add(
                companyAppUserId,
                "payhawk",
                null,
                null,
                2014,
                "Payment service. Payhawk Payhawk Fintech. Cool.",
                null,
                "Hristo Borisov",
                200,
                false,
                false,
                "fewfwefew",
                countryId,
                "somecooldomain.com");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.Add(
                companyAppUserId,
                "payhawk",
                null,
                null,
                2014,
                "Payment service. Payhawk Payhawk Fintech. Cool.",
                null,
                "Hristo Borisov",
                200,
                false,
                false,
                specializationId,
                "fewf",
                "somecooldomain.com");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_CompanyId()
        {
            SeedDatabase();

            var actualResult = companyService.Add(
                companyAppUserId,
                "payhawk",
                null,
                null,
                2014,
                "Payment service. Payhawk Payhawk Fintech. Cool.",
                null,
                "Hristo Borisov",
                200,
                false,
                false,
                specializationId,
                countryId,
                "somecooldomain.com");

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.Edit(
                "fwefwef",
                "new name",
                null,
                null,
                2022,
                "340tj93409jgw09efj0rgjreg",
                null,
                "Boris Hristov",
                250,
                false,
                false,
                specializationId,
                countryId,
                "somedomain.com");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.Edit(
                companyId,
                "new name",
                null,
                null,
                2022,
                "340tj93409jgw09efj0rgjreg",
                null,
                "Boris Hristov",
                250,
                false,
                false,
                specializationId,
                countryId,
                "somedomain.com");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenSpecializationIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.Edit(
                companyId,
                "new name",
                null,
                null,
                2022,
                "340tj93409jgw09efj0rgjreg",
                null,
                "Boris Hristov",
                250,
                false,
                false,
                "fewffwe[fkeopwkf3",
                countryId,
                "somedomain.com");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.Edit(
                companyId,
                "new name",
                null,
                null,
                2022,
                "340tj93409jgw09efj0rgjreg",
                null,
                "Boris Hristov",
                250,
                false,
                false,
                specializationId,
                "fwepofk39",
                "somedomain.com");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldChange_ModifiedOnProperty_AndReturn_True()
        {
            SeedDatabase();

            var actualResult = companyService.Edit(
                companyId,
                "new name",
                null,
                null,
                2022,
                "340tj93409jgw09efj0rgjreg",
                null,
                "Boris Hristov",
                250,
                false,
                false,
                specializationId,
                countryId,
                "somedomain.com");

            var actualCompany = data.Companies.Find(companyId);

            Assert.IsTrue(actualResult);
            Assert.IsNotNull(actualCompany.ModifiedOn);
        }

        [Test]
        public void AddCandidateToInterns_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.AddCandidateToInterns(
                "gwrger",
                companyId,
                "Junior Full-Stack Developer");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToInterns_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();
            SeedCandidate();

            candidateService = new CandidateService(data, null);

            candidateService.Delete(candidateId);

            var actualResult = companyService.AddCandidateToInterns(
                candidateId,
                companyId,
                "Junior Full-Stack Developer");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToInterns_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            SeedCandidate();

            var actualResult = companyService.AddCandidateToInterns(
                candidateId,
                "ferpojo",
                "Junior Full-Stack Developer");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToInterns_ShouldReturn_FalseWhenCompanyIsDeleted()
        {
            SeedDatabase();
            SeedCandidate();

            companyService.Delete(companyId);

            var actualResult = companyService.AddCandidateToInterns(
                candidateId,
                companyId,
                "Junior Full-Stack Developer");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToInterns_ShouldReturn_True()
        {
            SeedDatabase();
            SeedCandidate();

            var actualResult = companyService.AddCandidateToInterns(
                candidateId,
                companyId,
                "Junior Full-Stack Developer");

            var actualCompanyIdProperty = data.Candidates.Find(candidateId)?.CompanyId;

            Assert.IsTrue(actualResult);
            Assert.That(actualCompanyIdProperty, Is.EqualTo(companyId));
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            var actualResult = companyService.Delete("fwef");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.Delete(companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_True()
        {
            SeedDatabase();

            var actualDelete = companyService.Delete(companyId);
            var actualExists = companyService.Exists(companyId);

            Assert.IsTrue(actualDelete);
            Assert.IsFalse(actualExists);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            var actualResult = companyService.Exists("fewf");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.Exists(companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = companyService.Exists(companyId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenCompanyIdDoesNotMatch()
        {
            var actualResult = companyService.GetDetailsModel("wergr");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.GetDetailsModel(companyId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_CompanyDetailsViewModel()
        {
            SeedDatabase();

            var expectedModel = new CompanyDetailsViewModel()
            {
                Id = companyId,
                Name = "Payhawk",
                Founded = 2014,
                Description = "Providing payment services. Fintech. Cool.",
                CEO = "Hristo Borisov",
                EmployeesCount = 200,
                Specialization = "Software Development",
                Country = "Bulgaria"
            };

            var actualModel = companyService.GetDetailsModel(companyId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Name").EqualTo(expectedModel.Name)
                                   & Has.Property("Founded").EqualTo(expectedModel.Founded)
                                   & Has.Property("Description").EqualTo(expectedModel.Description)
                                   & Has.Property("CEO").EqualTo(expectedModel.CEO)
                                   & Has.Property("EmployeesCount").EqualTo(expectedModel.EmployeesCount)
                                   & Has.Property("Specialization").EqualTo(expectedModel.Specialization)
                                   & Has.Property("Country").EqualTo(expectedModel.Country));

            Assert.IsNull(actualModel.ImageUrl);
            Assert.IsNull(actualModel.RevenueUSD);
            Assert.IsNull(actualModel.WebsiteUrl);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenCompanyIdDoesNotMatch()
        {
            var actualResult = companyService.GetEditModel("gerg");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.GetEditModel(companyId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_EditCompanyFormModel()
        {
            SeedDatabase();

            var expectedModel = new EditCompanyFormModel()
            {
                Id = companyId,
                Name = "Payhawk",
                Founded = 2014,
                Description = "Providing payment services. Fintech. Cool.",
                CEO = "Hristo Borisov",
                EmployeesCount = 200,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            var actualModel = companyService.GetEditModel(companyId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Name").EqualTo(expectedModel.Name)
                                   & Has.Property("Founded").EqualTo(expectedModel.Founded)
                                   & Has.Property("Description").EqualTo(expectedModel.Description)
                                   & Has.Property("CEO").EqualTo(expectedModel.CEO)
                                   & Has.Property("EmployeesCount").EqualTo(expectedModel.EmployeesCount)
                                   & Has.Property("SpecializationId").EqualTo(expectedModel.SpecializationId)
                                   & Has.Property("CountryId").EqualTo(expectedModel.CountryId));

            Assert.IsNull(actualModel.ImageUrl);
            Assert.IsNull(actualModel.RevenueUSD);
            Assert.IsNull(actualModel.WebsiteUrl);
        }

        [Test]
        public void GetCompaniesSelectOptions_ShouldReturn_EmptyCollectionWhenCompaniesAreDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.GetCompaniesSelectOptions();

            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void GetCompaniesSelectOptions_ShouldReturn_CompanySelectOptionsViewModel()
        {
            SeedDatabase();

            var actualFirstCompany = companyService
                .GetCompaniesSelectOptions()
                .FirstOrDefault();

            var expectedModel = new CompanySelectOptionsViewModel
            {
                Id = companyId,
                Name = "Payhawk"
            };

            Assert.That(actualFirstCompany, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Name").EqualTo(expectedModel.Name));
        }

        [Test]
        public void All_ShouldReturn_CompanyListingQueryModel_WithEmptyCompaniesCollection_WhenSpecializationIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.All(
                null,
                "fwepofwepf",
                null,
                null,
                false,
                false,
                1,
                6);

            Assert.IsEmpty(actualResult.Companies);
        }

        [Test]
        public void All_ShouldReturn_CompanyListingQueryModel_WithEmptyCompaniesCollection_WhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = companyService.All(
                null,
                null,
                "fweokew",
                null,
                false,
                false,
                1,
                6);

            Assert.IsEmpty(actualResult.Companies);
        }

        [Test]
        public void All_ShouldReturn_CompanyListingQueryModel_WithEmptyCompaniesCollection_WhenAllCompaniesAreDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = companyService.All(
                null,
                null,
                null,
                null,
                false,
                false,
                1,
                6);

            Assert.IsEmpty(actualResult.Companies);
        }

        [Test]
        public void All_ShouldSanitizeName_AndReturn_CompanyListingQueryModel_WithFilteredCompaniesCollection()
        {
            SeedDatabase();

            var actualResult = companyService.All(
                "payhawk<script>this part will be removed</script>",
                null,
                null,
                null,
                false,
                false,
                1,
                6);

            Assert.That(actualResult.Companies.Count(), Is.EqualTo(1));
        }

        [Test]
        public void All_ShouldReturn_CompanyListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            var actualResultOne = companyService.All(
                null,
                null,
                null,
                null,
                false,
                false,
                -1,
                -11);

            var actualResultTwo = companyService.All(
                null,
                null,
                null,
                null,
                false,
                false,
                0,
                999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.CompaniesPerPage, Is.EqualTo(6));
            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.CompaniesPerPage, Is.EqualTo(96));

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

            // Add Identity User
            var companyAppUser = new ApplicationUser
            {
                Email = "payhawk@gmail.com",
                UserName = "payhawk@gmail.com"
            };

            companyAppUserId = companyAppUser.Id;

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
                SpecializationId = specialization.Id,
                CountryId = country.Id
            };

            companyId = company.Id;

            data.Companies.Add(company);
            data.SaveChanges();
        }

        private void SeedCandidate()
        {
            // Add Specializations, Countries
            var specialization = new Specialization
            {
                Name = "Software Development"
            };

            var country = new Country
            {
                Name = "Bulgaria"
            };

            data.Specializations.Add(specialization);
            data.Countries.Add(country);

            // Add Identity User
            var candidateAppUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre@gmail.com"
            };

            data.Users.Add(candidateAppUser);
            data.SaveChanges();

            // Add Candidate
            var candidate = new Candidate
            {
                FirstName = "Emre",
                LastName = "Ereceb",
                UserId = candidateAppUser.Id,
                BirthDate = DateTime.UtcNow.AddYears(-20),
                Gender = Internify.Data.Models.Enums.Gender.Male,
                SpecializationId = specialization.Id,
                CountryId = country.Id
            };

            candidateId = candidate.Id;

            data.Candidates.Add(candidate);
            data.SaveChanges();
        }
    }
}