namespace Internify.Tests.Services
{
    using Data;
    using Data.Models;
    using Internify.Services.Candidate;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.Candidate;
    using Models.ViewModels.Candidate;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CandidateServiceTests
    {
        private InternifyDbContext data;
        private ICandidateService candidateService;

        private string candidateId;
        private string companyId;
        private string candidateAppUserId;
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
            candidateService = new CandidateService(data, null);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void IsCandidate_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            var actualResult = candidateService.IsCandidate("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidate_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.IsCandidate(candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidate_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = candidateService.IsCandidate(candidateId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsCandidateByUserId_ShouldReturn_FalseWhenUserIdDoesNotMatch()
        {
            var actualResult = candidateService.IsCandidateByUserId("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateByUserId_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.IsCandidateByUserId(candidateAppUserId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateByUserId_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = candidateService.IsCandidateByUserId(candidateAppUserId);

            Assert.True(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_NullWhenUserIdDoesNotMatch()
        {
            var actualResult = candidateService.GetIdByUserId("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_NullWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.GetIdByUserId(candidateAppUserId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_CandidateId()
        {
            SeedDatabase();

            var actualResult = candidateService.GetIdByUserId(candidateAppUserId);

            Assert.That(actualResult, Is.EqualTo(candidateId));
        }

        [Test]
        public void Add_ShouldReturn_NullWhenUserIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateService.Add(
                "",
                "Emre",
                "Ereceb",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-20),
                Data.Models.Enums.Gender.Male,
                specializationId,
                countryId,
                "somewebsite.com");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenSpecializationIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateService.Add(
                candidateAppUserId,
                "Emre",
                "Ereceb",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-20),
                Data.Models.Enums.Gender.Male,
                "",
                countryId,
                "somewebsite.com");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateService.Add(
                candidateAppUserId,
                "Emre",
                "Ereceb",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-20),
                Data.Models.Enums.Gender.Male,
                specializationId,
                "",
                "somewebsite.com");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_CandidateId()
        {
            SeedDatabase();

            var actualResult = candidateService.Add(
                candidateAppUserId,
                "Emre",
                "Ereceb",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-20),
                Data.Models.Enums.Gender.Male,
                specializationId,
                countryId,
                "somewebsite.com");

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateService.Edit(
                "",
                "new first name",
                "new last name",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-21),
                Data.Models.Enums.Gender.NotSay,
                false,
                specializationId,
                countryId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.Edit(
                candidateId,
                "new first name",
                "new last name",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-21),
                Data.Models.Enums.Gender.NotSay,
                false,
                specializationId,
                countryId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenSpecializationIdDoesNotMatch()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.Edit(
                candidateId,
                "new first name",
                "new last name",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-21),
                Data.Models.Enums.Gender.NotSay,
                false,
                "",
                countryId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.Edit(
                candidateId,
                "new first name",
                "new last name",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-21),
                Data.Models.Enums.Gender.NotSay,
                false,
                specializationId,
                "");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldChange_ModifiedOnProperty_AndReturn_True()
        {
            SeedDatabase();

            var actualResult = candidateService.Edit(
                candidateId,
                "new first name",
                "new last name",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-21),
                Data.Models.Enums.Gender.NotSay,
                false,
                specializationId,
                countryId);

            var actualModifiedOnProperty = candidateService.GetDetailsModel(candidateId)?.ModifiedOn;

            Assert.IsTrue(actualResult);
            Assert.IsNotNull(actualModifiedOnProperty);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            var actualResult = candidateService.Delete("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.Delete("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_True()
        {
            SeedDatabase();

            var actualDeleteResult = candidateService.Delete(candidateId);
            var actualExistsResult = candidateService.Exists(candidateId);

            Assert.IsTrue(actualDeleteResult);
            Assert.IsFalse(actualExistsResult);
        }

        [Test]
        public void GetEmail_ShouldReturn_NullWhenCandidateIdDoesNotMatchOrCandidateIsDeleted()
        {
            var actualResult = candidateService.GetEmail("");

            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualDeletedResult = candidateService.GetEmail(candidateId);

            Assert.IsNull(actualResult);
            Assert.IsNull(actualDeletedResult);
        }

        [Test]
        public void IsCandidateAlreadyAnIntern_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            var actualResult = candidateService.IsCandidateAlreadyAnIntern("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateAlreadyAnIntern_ShouldReturn_FalseWhenCandidateDoesNotBelongToCompany()
        {
            SeedDatabase();

            var actualResult = candidateService.IsCandidateAlreadyAnIntern(candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateAlreadyAnIntern_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.IsCandidateAlreadyAnIntern(candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateAlreadyAnIntern_ShouldReturn_True()
        {
            SeedDatabase();

            SeedCompany();

            var candidate = data.Candidates.Find(candidateId);
            candidate.CompanyId = companyId;

            data.SaveChanges();

            var actualResult = candidateService.IsCandidateAlreadyAnIntern(candidateId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsCandidateInCompany_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            SeedCompany();

            var candidate = data.Candidates.Find(candidateId);
            candidate.CompanyId = companyId;

            data.SaveChanges();

            var actualResult = candidateService.IsCandidateInCompany(
                "",
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateInCompany_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            SeedCompany();

            var candidate = data.Candidates.Find(candidateId);
            candidate.CompanyId = companyId;

            data.SaveChanges();

            var actualResult = candidateService.IsCandidateInCompany(
                candidateId,
                "");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateInCompany_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            SeedCompany();

            var candidate = data.Candidates.Find(candidateId);
            candidate.CompanyId = companyId;

            data.SaveChanges();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.IsCandidateInCompany(
                candidateId,
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateInCompany_ShouldReturn_True()
        {
            SeedDatabase();

            SeedCompany();

            var candidate = data.Candidates.Find(candidateId);
            candidate.CompanyId = companyId;

            data.SaveChanges();

            var actualResult = candidateService.IsCandidateInCompany(
                candidateId,
                companyId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void RemoveFromCompany_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            var actualResult = candidateService.RemoveFromCompany("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void RemoveFromCompany_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.RemoveFromCompany(candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void RemoveFromCompany_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = candidateService.RemoveFromCompany(candidateId);
            var actualCompanyId = candidateService.GetDetailsModel(candidateId)?.CompanyId;

            Assert.IsTrue(actualResult);
            Assert.IsNull(actualCompanyId);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            var actualResult = candidateService.Exists("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.Exists(candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = candidateService.Exists(candidateId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenCandidateIdDoesNotMatch()
        {
            var actualResult = candidateService.GetDetailsModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.GetDetailsModel(candidateId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldHave_ModifiedOnPropertyNotNullAfterEdit()
        {
            SeedDatabase();

            candidateService.Edit(
                candidateId,
                "Emrr",
                "Errrec",
                null,
                null,
                null,
                DateTime.UtcNow.AddYears(-21),
                Data.Models.Enums.Gender.Male,
                false,
                specializationId,
                countryId);

            var actualModifiedOn = candidateService.GetDetailsModel(candidateId)?.ModifiedOn;

            Assert.IsNotNull(actualModifiedOn);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_CandidateDetailsViewModel()
        {
            SeedDatabase();

            var expectedModel = new CandidateDetailsViewModel
            {
                Id = candidateId,
                FirstName = "Emre",
                LastName = "Ereceb",
                Gender = "Male",
                IsAvailableMessage = "✔️ Open for offers",
                Specialization = "Software Development",
                Country = "Bulgaria",
            };

            var actualModel = candidateService.GetDetailsModel(candidateId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("FirstName").EqualTo(expectedModel.FirstName)
                                   & Has.Property("LastName").EqualTo(expectedModel.LastName)
                                   & Has.Property("Gender").EqualTo(expectedModel.Gender)
                                   & Has.Property("IsAvailableMessage").EqualTo(expectedModel.IsAvailableMessage)
                                   & Has.Property("Specialization").EqualTo(expectedModel.Specialization)
                                   & Has.Property("Country").EqualTo(expectedModel.Country));

            Assert.IsNull(actualModel.ImageUrl);
            Assert.IsNull(actualModel.WebsiteUrl);
            Assert.IsNull(actualModel.Description);
            Assert.IsNotNull(actualModel.BirthDate);
            Assert.IsNotNull(actualModel.CreatedOn);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenCandidateIdDoesNotMatch()
        {
            var actualResult = candidateService.GetEditModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.GetEditModel(candidateId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_EditCandidateFormModel()
        {
            SeedDatabase();

            var expectedModel = new EditCandidateFormModel
            {
                Id = candidateId,
                FirstName = "Emre",
                LastName = "Ereceb",
                Gender = Data.Models.Enums.Gender.Male,
                IsAvailable = true,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            var actualModel = candidateService.GetEditModel(candidateId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("FirstName").EqualTo(expectedModel.FirstName)
                                   & Has.Property("LastName").EqualTo(expectedModel.LastName)
                                   & Has.Property("Gender").EqualTo(expectedModel.Gender)
                                   & Has.Property("IsAvailable").EqualTo(expectedModel.IsAvailable)
                                   & Has.Property("SpecializationId").EqualTo(expectedModel.SpecializationId)
                                   & Has.Property("CountryId").EqualTo(expectedModel.CountryId));

            Assert.IsNull(actualModel.ImageUrl);
            Assert.IsNull(actualModel.WebsiteUrl);
            Assert.IsNull(actualModel.Description);
            Assert.IsNotNull(actualModel.BirthDate);
        }

        [Test]
        public void All_ShouldReturn_CandidateListingQueryModel_WithEmptyCandidatesCollection_WhenSpecializationIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateService.All(
                null,
                null,
                "dfwefwef",
                countryId,
                true,
                1,
                6);

            Assert.IsEmpty(actualResult.Candidates);
        }

        [Test]
        public void All_ShouldReturn_CandidateListingQueryModel_WithEmptyCandidatesCollection_WhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateService.All(
                null,
                null,
                specializationId,
                "efef3f",
                true,
                1,
                6);

            Assert.IsEmpty(actualResult.Candidates);
        }

        [Test]
        public void All_ShouldReturn_CandidateListingQueryModel_WithEmptyCandidatesCollection_WhenAllCandidatesAreDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.All(
                null,
                null,
                null,
                null,
                true,
                1,
                6);

            Assert.IsEmpty(actualResult.Candidates);
        }

        [Test]
        public void All_ShouldSanitizeFirstName_AndReturn_CandidateListingQueryModel_WithFilteredCandidatesCollection()
        {
            SeedDatabase();

            var actualResult = candidateService.All(
                "emre",
                null,
                specializationId,
                countryId,
                true,
                1,
                6);

            Assert.That(actualResult.Candidates.Count(), Is.EqualTo(1));
        }

        [Test]
        public void All_ShouldSanitizeLastName_AndReturn_CandidateListingQueryModel_WithFilteredCandidatesCollection()
        {
            SeedDatabase();

            var actualResult = candidateService.All(
                null,
                "ereceb",
                specializationId,
                countryId,
                true,
                1,
                6);

            Assert.That(actualResult.Candidates.Count(), Is.EqualTo(1));
        }

        [Test]
        public void All_ShouldReturn_CandidateListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            var actualResultOne = candidateService.All(
                null,
                null,
                null,
                null,
                true,
                -1,
                -1);

            var actualResultTwo = candidateService.All(
                null,
                null,
                null,
                null,
                true,
                0,
                999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.CandidatesPerPage, Is.EqualTo(6));
            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.CandidatesPerPage, Is.EqualTo(96));
        }

        [Test]
        public void GetCandidatesByCompany_ShouldReturn_InternListingQueryModel_WithEmptyInternsCollection_WhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateService.GetCandidatesByCompany(
                "fwefewf",
                null,
                null,
                null,
                1,
                6);

            Assert.IsEmpty(actualResult.Interns);
        }

        [Test]
        public void GetCandidatesByCompany_ShouldReturn_InternListingQueryModel_WithEmptyInternsCollection_WhenCandidatesAreDeleted()
        {
            SeedDatabase();
            SeedCompany();

            candidateService.Delete(candidateId);

            var actualResult = candidateService.GetCandidatesByCompany(
                companyId,
                null,
                null,
                null,
                1,
                6);

            Assert.IsEmpty(actualResult.Interns);
        }

        [Test]
        public void GetCandidatesByCompany_ShouldSanitizeFirstName_AndReturn_InternListingQueryModel_WithFilteredInternsCollection()
        {
            SeedDatabase();
            SeedCompany();

            var actualResult = candidateService.GetCandidatesByCompany(
                null,
                "emre",
                null,
                null,
                1,
                6);

            Assert.That(actualResult.Interns.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetCandidatesByCompany_ShouldSanitizeLastName_AndReturn_InternListingQueryModel_WithFilteredInternsCollection()
        {
            SeedDatabase();
            SeedCompany();

            var actualResult = candidateService.GetCandidatesByCompany(
                null,
                null,
                "erec",
                null,
                1,
                6);

            Assert.That(actualResult.Interns.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetCandidatesByCompany_ShouldSanitizeInternshipRole_AndReturn_InternListingQueryModel_WithFilteredInternsCollection()
        {
            SeedDatabase();
            SeedCompany();

            var candidate = data.Candidates.Find(candidateId);
            candidate.CompanyId = companyId;
            candidate.InternshipRole = "Junior Full-Stack Developer";
            data.SaveChanges();

            var actualResult = candidateService.GetCandidatesByCompany(
                companyId,
                null,
                null,
                "junior",
                1,
                6);

            Assert.That(actualResult.Interns.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetCandidatesByCompany_ShouldReturn_InternListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            var actualResultOne = candidateService.GetCandidatesByCompany(
                null,
                null,
                null,
                null,
                -1,
                -1);

            var actualResultTwo = candidateService.GetCandidatesByCompany(
                null,
                null,
                null,
                null,
                0,
                999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.InternsPerPage, Is.EqualTo(6));
            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.InternsPerPage, Is.EqualTo(96));
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

            // Add Identity Users
            var candidateAppUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre@gmail.com"
            };

            candidateAppUserId = candidateAppUser.Id;

            data.Users.Add(candidateAppUser);
            data.SaveChanges();

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
        }

        private void SeedCompany()
        {
            var companyAppUser = new ApplicationUser
            {
                Email = "payhawk@gmail.com",
                UserName = "payhawk@gmail.com"
            };

            data.Users.Add(companyAppUser);
            data.SaveChanges();

            var company = new Company
            {
                Name = "Payhawk",
                UserId = companyAppUser.Id,
                Founded = 2014,
                Description = "Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk Payhawk",
                RevenueUSD = 1000000000,
                CEO = "Hristo Borisov",
                EmployeesCount = 200,
                SpecializationId = specializationId,
                CountryId = countryId
            };

            companyId = company.Id;

            data.Companies.Add(company);
            data.SaveChanges();
        }
    }
}