namespace Internify.Tests.Services
{
    using Data;
    using Data.Models;
    using Internify.Services.Administrator;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;
    using System.Linq;

    public class AdministratorServiceTests
    {
        private InternifyDbContext data;
        private IAdministratorService administratorService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            administratorService = new AdministratorService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void GetUserCountPerRole_ShouldReturn_ObjectWithCorrectPropertyValues()
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

            var candidateAppUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre123"
            };

            var candidateTwoAppUser = new ApplicationUser
            {
                Email = "vasilev@gmail.com",
                UserName = "vasilev123"
            };

            var companyAppUser = new ApplicationUser
            {
                Email = "payhawk@gmail.com",
                UserName = "payhawk@gmail.com"
            };

            var universityAppUser = new ApplicationUser
            {
                Email = "softuni@gmail.com",
                UserName = "softuni@gmail.com"
            };

            data.Users.Add(candidateAppUser);
            data.Users.Add(candidateTwoAppUser);
            data.Users.Add(companyAppUser);
            data.Users.Add(universityAppUser);

            data.SaveChanges();

            // Add Users (with their relations)

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

            var candidateTwo = new Candidate
            {
                FirstName = "Atanas",
                LastName = "Vasilev",
                UserId = candidateTwoAppUser.Id,
                BirthDate = DateTime.UtcNow.AddYears(-20),
                Gender = Data.Models.Enums.Gender.Male,
                SpecializationId = specialization.Id,
                CountryId = country.Id
            };

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

            var university = new University
            {
                Name = "Software University",
                UserId = universityAppUser.Id,
                ImageUrl = "https://yt3.ggpht.com/ytc/AKedOLSp0TNfguM4s9wg2yTKv-_wzdu9VAZxAPnWSNYWKo4=s900-c-k-c0x00ffffff-no-rj",
                WebsiteUrl = "softuni.bg",
                Founded = 2013,
                Type = Data.Models.Enums.Type.Private,
                Description = "fttgththth",
                CountryId = country.Id
            };

            data.Candidates.Add(candidate);
            data.Candidates.Add(candidateTwo);
            data.Companies.Add(company);
            data.Universities.Add(university);

            data.SaveChanges();

            var candidatesCount = data.Candidates.Where(x => !x.IsDeleted).Count();
            var companiesCount = data.Companies.Where(x => !x.IsDeleted).Count();
            var universitiesCount = data.Universities.Where(x => !x.IsDeleted).Count();

            var usersPerRole = administratorService.GetUserCountPerRole();

            Assert.That(usersPerRole.Candidates, Is.EqualTo(candidatesCount));
            Assert.That(usersPerRole.Companies, Is.EqualTo(companiesCount));
            Assert.That(usersPerRole.Universities, Is.EqualTo(universitiesCount));
        }
    }
}