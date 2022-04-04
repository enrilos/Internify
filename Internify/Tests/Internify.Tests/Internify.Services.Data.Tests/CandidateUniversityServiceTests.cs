namespace Internify.Services.Data.Tests
{
    using CandidateUniversity;
    using Internify.Data;
    using Internify.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;

    public class CandidateUniversityServiceTests
    {
        private InternifyDbContext data;
        private ICandidateUniversityService candidateUniversityService;

        private string candidateId;
        private string universityId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            candidateUniversityService = new CandidateUniversityService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void IsCandidateInUniversityAlumni_ShouldReturn_FalseWhenUniversityIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateUniversityService.IsCandidateInUniversityAlumni(
                "adfew",
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateInUniversityAlumni_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateUniversityService.IsCandidateInUniversityAlumni(
                universityId,
                "fefwef");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsCandidateInUniversityAlumni_ShouldReturn_True()
        {
            SeedDatabase();

            candidateUniversityService.AddCandidateToAlumni(
                universityId,
                candidateId);

            var actualResult = candidateUniversityService.IsCandidateInUniversityAlumni(
                universityId,
                candidateId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void AddCandidateToAlumni_ShouldReturn_FalseWhenUniversityIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateUniversityService.AddCandidateToAlumni(
                "dfwef",
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToAlumni_ShouldReturn_FalseWhenUniversityIsDeleted()
        {
            SeedDatabase();

            var university = data.Universities.Find(universityId);
            data.Universities.Remove(university);

            data.SaveChanges();

            var actualResult = candidateUniversityService.AddCandidateToAlumni(
                universityId,
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToAlumni_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateUniversityService.AddCandidateToAlumni(
                universityId,
                "rfwef");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToAlumni_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            var candidate = data.Candidates.Find(candidateId);
            data.Candidates.Remove(candidate);

            data.SaveChanges();

            var actualResult = candidateUniversityService.AddCandidateToAlumni(
                universityId,
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void AddCandidateToAlumni_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = candidateUniversityService.AddCandidateToAlumni(
                universityId,
                candidateId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void RemoveCandidateFromAlumni_ShouldReturn_FalseWhenUniversityIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateUniversityService.RemoveCandidateFromAlumni(
                "dfwef",
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void RemoveCandidateFromAlumni_ShouldReturn_FalseWhenUniversityIsDeleted()
        {
            SeedDatabase();

            var university = data.Universities.Find(universityId);
            data.Universities.Remove(university);

            data.SaveChanges();

            var actualResult = candidateUniversityService.RemoveCandidateFromAlumni(
                universityId,
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void RemoveCandidateFromAlumni_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = candidateUniversityService.RemoveCandidateFromAlumni(
                universityId,
                "rfwef");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void RemoveCandidateFromAlumni_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            var candidate = data.Candidates.Find(candidateId);
            data.Candidates.Remove(candidate);

            data.SaveChanges();

            var actualResult = candidateUniversityService.RemoveCandidateFromAlumni(
                universityId,
                candidateId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void RemoveCandidateFromAlumni_ShouldReturn_True()
        {
            SeedDatabase();

            candidateUniversityService.AddCandidateToAlumni(
                universityId,
                candidateId);

            var actualResult = candidateUniversityService.RemoveCandidateFromAlumni(
                universityId,
                candidateId);

            Assert.IsTrue(actualResult);
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
            var universityAppUser = new ApplicationUser
            {
                Email = "softuni@gmail.com",
                UserName = "softuni123"
            };

            var candidateAppUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre123"
            };

            data.Users.Add(universityAppUser);
            data.Users.Add(candidateAppUser);
            data.SaveChanges();

            // Add University
            var university = new University
            {
                Name = "Software University",
                UserId = universityAppUser.Id,
                ImageUrl = "www.somedomain.com/img.jpg",
                WebsiteUrl = "www.somedomain.com",
                Founded = 2013,
                Type = Internify.Data.Models.Enums.Type.Private,
                Description = "Quality education at an affordable price.",
                CountryId = country.Id
            };

            universityId = university.Id;

            data.Universities.Add(university);

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