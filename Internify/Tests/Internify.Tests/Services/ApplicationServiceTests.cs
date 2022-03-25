namespace Internify.Tests.Services
{
    using Data;
    using Data.Models;
    using Internify.Services.Application;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels.Application;
    using NUnit.Framework;
    using System;

    public class ApplicationServiceTests
    {
        private InternifyDbContext data;
        private IApplicationService applicationService;

        private string internshipId;
        private string companyId;
        private string candidateId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            applicationService = new ApplicationService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_NullWhenApplicationIdDoesNotMatch()
        {
            var actualResult = applicationService.Edit(
                "",
                "This application does not exist This application does not exist This application does not exist");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_NullWhenApplicationIsDeleted()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            applicationService.Delete(id);

            var actualResult = applicationService.Edit(
                id,
                "fwiefwpefowekfpwefkopwe");

            Assert.IsNull(actualResult);
        }
        
        [Test]
        public void Edit_ShouldChange_ModifiedOnProperty_AndReturn_ApplicationIdWhenSuccessful()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.Edit(
                id,
                "new cover letter new cover letter new cover letter new cover letter new cover letter");

            var actualModifiedOnProperty = applicationService.GetDetailsModel(id)?.ModifiedOn;

            Assert.That(actualResult, Is.EqualTo(id));
            Assert.IsNotNull(actualModifiedOnProperty);
        }

        [Test]
        public void HasCandidateApplied_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.HasCandidateApplied(
                "",
                internshipId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void HasCandidateApplied_ShouldReturn_FalseWhenInternshipIdDoesNotMatch()
        {
            SeedDatabase();

            applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.HasCandidateApplied(
                candidateId,
                "");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void HasCandidateApplied_ShouldReturn_FalseWhenApplicationIsDeleted()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            applicationService.Delete(id);

            var actualResult = applicationService.HasCandidateApplied(
                candidateId,
                internshipId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void HasCandidateApplied_ShouldReturn_True()
        {
            SeedDatabase();

            applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.HasCandidateApplied(
                candidateId,
                internshipId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenApplicationIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = applicationService.Exists("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenApplicationIsDeleted()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            applicationService.Delete(id);

            var actualResult = applicationService.Exists(id);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.Exists(id);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsApplicationOwnedByCandidate_ShouldReturn_FalseWhenApplicationIdDoesNotMatch()
        {
            SeedDatabase();

            applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.IsApplicationOwnedByCandidate(
                "",
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsApplicationOwnedByCandidate_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.IsApplicationOwnedByCandidate(
                id,
                "");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsApplicationOwnedByCandidate_ShouldReturn_FalseWhenApplicationIsDeleted()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            applicationService.Delete(id);

            var actualResult = applicationService.IsApplicationOwnedByCandidate(
                id,
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsApplicationOwnedByCandidate_ShouldReturn_True()
        {
            SeedDatabase();

            var id = applicationService.Add(
                internshipId,
                candidateId,
                "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.IsApplicationOwnedByCandidate(
                id,
                candidateId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void DoesApplicationBelongToCompanyInternship_ShouldReturn_FalseWhenApplicationIdDoesNotMatch()
        {
            SeedDatabase();

            applicationService.Add(
               internshipId,
               candidateId,
               "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.DoesApplicationBelongToCompanyInternship(
                "",
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void DoesApplicationBelongToCompanyInternship_ShouldReturn_FalseWhenInternshipCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var id = applicationService.Add(
              internshipId,
              candidateId,
              "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.DoesApplicationBelongToCompanyInternship(
                id,
                "");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void DoesApplicationBelongToCompanyInternship_ShouldReturn_FalseWhenApplicationIsDeleted()
        {
            SeedDatabase();

            var id = applicationService.Add(
              internshipId,
              candidateId,
              "This is cover letter This is a cover letter This is a cover letter");

            applicationService.Delete(id);

            var actualResult = applicationService.DoesApplicationBelongToCompanyInternship(
                id,
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void DoesApplicationBelongToCompanyInternship_ShouldReturn_True()
        {
            SeedDatabase();

            var id = applicationService.Add(
              internshipId,
              candidateId,
              "This is cover letter This is a cover letter This is a cover letter");

            var actualResult = applicationService.DoesApplicationBelongToCompanyInternship(
                id,
                companyId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenApplicationIdDoesNotMatch()
        {
            var actualResult = applicationService.Delete("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_True()
        {
            SeedDatabase();

            var id = applicationService.Add(
              internshipId,
              candidateId,
              "This is cover letter This is a cover letter This is a cover letter");

            var actualDeleteResult = applicationService.Delete(id);
            var actualExistsResult = applicationService.Exists(id);

            Assert.IsTrue(actualDeleteResult);
            Assert.IsFalse(actualExistsResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenApplicationIdDoesNotMatch()
        {
            var actualResult = applicationService.GetDetailsModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenApplicationIsDeleted()
        {
            SeedDatabase();

            var id = applicationService.Add(
              internshipId,
              candidateId,
              "This is cover letter This is a cover letter This is a cover letter");

            applicationService.Delete(id);

            var actualResult = applicationService.GetDetailsModel(id);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldHave_ModifiedOnPropertyNotNullAfterEdit()
        {
            SeedDatabase();

            var id = applicationService.Add(
              internshipId,
              candidateId,
              "This is cover letter This is a cover letter This is a cover letter");

            applicationService.Edit(
                id,
                "new cover letter");

            var actualModifiedOn = applicationService.GetDetailsModel(id)?.ModifiedOn;

            Assert.IsNotNull(actualModifiedOn);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_ApplicationDetailsViewModel()
        {
            SeedDatabase();

            var id = applicationService.Add(
              internshipId,
              candidateId,
              "This is cover letter This is a cover letter This is a cover letter");

            var actualModel = applicationService.GetDetailsModel(id);
            var expectedModel = new ApplicationDetailsViewModel
            {
                Id = id,
                Role = "Junior Full-Stack Developer",
                InternshipId = internshipId,
                Company = "Payhawk",
                CompanyId = companyId,
                CoverLetter = "This is cover letter This is a cover letter This is a cover letter"
            };

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Role").EqualTo(expectedModel.Role)
                                   & Has.Property("InternshipId").EqualTo(expectedModel.InternshipId)
                                   & Has.Property("Company").EqualTo(expectedModel.Company)
                                   & Has.Property("CompanyId").EqualTo(expectedModel.CompanyId)
                                   & Has.Property("CoverLetter").EqualTo(expectedModel.CoverLetter));

            Assert.IsNotNull(actualModel.CreatedOn);
        }

        [Test]
        public void GetDetailsModelForCompany_ShouldReturn_NullWhenApplicationIdDoesNotMatch()
        {

        }

        [Test]
        public void GetDetailsModelForCompany_ShouldReturn_NullWhenApplicationIsDeleted()
        {

        }

        [Test]
        public void GetDetailsModelForCompany_ShouldReturn_ApplicationForCompanyDetailsViewModel()
        {

        }

        private void SeedDatabase()
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

            // Add Identity Users
            var companyAppUser = new ApplicationUser
            {
                Email = "payhawk@gmail.com",
                UserName = "payhawk123"
            };

            var candidateAppUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre123"
            };

            data.Users.Add(companyAppUser);
            data.Users.Add(candidateAppUser);
            data.SaveChanges();

            // Add Company
            var company = new Company
            {
                Name = "Payhawk",
                UserId = companyAppUser.Id,
                Founded = 2014,
                Description = "Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk",
                RevenueUSD = 1000000000,
                CEO = "Hristo Borisov",
                EmployeesCount = 200,
                SpecializationId = specialization.Id,
                CountryId = country.Id
            };

            companyId = company.Id;

            data.Companies.Add(company);

            // Add Candidate
            var candidate = new Candidate
            {
                FirstName = "Emre",
                LastName = "Ereceb",
                UserId = candidateAppUser.Id,
                BirthDate = DateTime.UtcNow.AddYears(-20),
                Gender = Data.Models.Enums.Gender.Male,
                SpecializationId = specialization.Id,
                CountryId = country.Id
            };

            candidateId = candidate.Id;

            data.Candidates.Add(candidate);
            data.SaveChanges();

            // Add Internship
            var internship = new Internship
            {
                Role = "Junior Full-Stack Developer",
                Description = "lorem lorem lorem lorem lorem lorem lorem lorem lorem lorem lorem lorem lorem lorem",
                CompanyId = company.Id,
                CountryId = country.Id,
            };

            internshipId = internship.Id;

            data.Internships.Add(internship);
            data.SaveChanges();
        }
    }
}