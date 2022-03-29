namespace Internify.Tests.Services
{
    using Data;
    using Data.Models;
    using Internify.Services.Candidate;
    using Internify.Services.Company;
    using Internify.Services.Review;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;
    using System.Linq;

    public class ReviewServiceTests
    {
        private InternifyDbContext data;
        private IReviewService reviewService;
        private ICandidateService candidateService;
        private ICompanyService companyService;

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
            reviewService = new ReviewService(data);
            candidateService = new CandidateService(data, null);
            companyService = new CompanyService(data, null);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ShouldReturn_FalseWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = reviewService.Add(
                "frwfwe",
                companyId,
                "Nice company",
                5,
                "Remarkable opportunities for improvement.");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_FalseWhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = reviewService.Add(
                candidateId,
                companyId,
                "Nice company",
                5,
                "Remarkable opportunities for improvement.");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = reviewService.Add(
                candidateId,
                "ferfe",
                "Nice company",
                5,
                "Remarkable opportunities for improvement.");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_FalseWhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = reviewService.Add(
                candidateId,
                companyId,
                "Nice company",
                5,
                "Remarkable opportunities for improvement.");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_True()
        {
            SeedDatabase();

             var actualResult = reviewService.Add(
                candidateId,
                companyId,
                "Nice company",
                5,
                "Remarkable opportunities for improvement.");

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void CompanyReviews_ShouldReturn_ReviewListingQueryModel_WithEmptyReviewsCollection_WhenCompanyIdDoesNotMatch()
        {
            var actualResult = reviewService.CompanyReviews(
                "ferf",
                null,
                null,
                1,
                6);

            Assert.IsEmpty(actualResult.Reviews);
        }

        [Test]
        public void CompanyReviews_ShouldReturn_ReviewListingQueryModel_WithEmptyReviewsCollection_WhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = reviewService.CompanyReviews(
                companyId,
                null,
                null,
                1,
                6);

            Assert.IsEmpty(actualResult.Reviews);
        }

        [Test]
        public void CompanyReviews_ShouldSanitizeTitle_AndReturn_ReviewListingQueryModel_WithFilteredReviewsCollection()
        {
            SeedDatabase();

            // Add some more reviews.
            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            var actualResult = reviewService.CompanyReviews(
                companyId,
                null,
                null,
                1,
                6);

            Assert.That(actualResult.Reviews.Count(), Is.EqualTo(6));
        }

        [Test]
        public void CompanyReviews_ShouldReturn_ReviewListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            SeedDatabase();

            // Add some more reviews.
            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            var actualResultOne = reviewService.CompanyReviews(
               companyId,
               null,
               null,
               -1,
               -1);

            var actualResultTwo = reviewService.CompanyReviews(
               companyId,
               "moderate",
               null,
               0,
               999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.ReviewsPerPage, Is.EqualTo(6));
            Assert.That(actualResultOne.Reviews.Count(), Is.EqualTo(6));

            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.ReviewsPerPage, Is.EqualTo(96));
            Assert.That(actualResultTwo.Reviews.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CandidateReviews_ShouldReturn_ReviewListingQueryModel_WithEmptyReviewsCollection_WhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = reviewService.CandidateReviews(
                "fwepojfwp",
                companyId,
                null,
                null,
                1,
                6);

            Assert.IsEmpty(actualResult.Reviews);
        }

        [Test]
        public void CandidateReviews_ShouldReturn_ReviewListingQueryModel_WithEmptyReviewsCollection_WhenCandidateIsDeleted()
        {
            SeedDatabase();

            candidateService.Delete(candidateId);

            var actualResult = reviewService.CandidateReviews(
                candidateId,
                companyId,
                null,
                null,
                1,
                6);

            Assert.IsEmpty(actualResult.Reviews);
        }

        [Test]
        public void CandidateReviews_ShouldReturn_ReviewListingQueryModel_WithEmptyReviewsCollection_WhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = reviewService.CandidateReviews(
                candidateId,
                "fwef",
                null,
                null,
                1,
                6);

            Assert.IsEmpty(actualResult.Reviews);
        }

        [Test]
        public void CandidateReviews_ShouldReturn_ReviewListingQueryModel_WithEmptyReviewsCollection_WhenCompanyIsDeleted()
        {
            SeedDatabase();

            companyService.Delete(companyId);

            var actualResult = reviewService.CandidateReviews(
               candidateId,
               companyId,
               null,
               null,
               1,
               6);

            Assert.IsEmpty(actualResult.Reviews);
        }

        [Test]
        public void CandidateReviews_ShouldSanitizeTitle_AndReturn_ReviewListingQueryModel_WithFilteredReviewsCollection()
        {
            SeedDatabase();

            // Add some more reviews.
            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            var actualResult = reviewService.CandidateReviews(
               candidateId,
               companyId,
               "moderate",
               null,
               1,
               6);

            Assert.That(actualResult.Reviews.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CandidateReviews_ShouldReturn_ReviewListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            SeedDatabase();

            // Add some more reviews.
            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "Superb work environment",
                5,
                "[pewkf[wekfwe[we");

            reviewService.Add(
                candidateId,
                companyId,
                "g34g34g34",
                4,
                "23rgergerg");

            reviewService.Add(
                candidateId,
                companyId,
                "thertg34g",
                2,
                "4t34t23t");

            reviewService.Add(
                candidateId,
                companyId,
                "Moderate work environment",
                3,
                "[pewkf[wekfwe[we");


            var actualResultOne = reviewService.CandidateReviews(
               candidateId,
               companyId,
               null,
               null,
               -1,
               -1);

            var actualResultTwo = reviewService.CandidateReviews(
               candidateId,
               companyId,
               "moderate",
               null,
               0,
               999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.ReviewsPerPage, Is.EqualTo(6));
            Assert.That(actualResultOne.Reviews.Count(), Is.EqualTo(6));

            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.ReviewsPerPage, Is.EqualTo(96));
            Assert.That(actualResultTwo.Reviews.Count(), Is.EqualTo(3));
        }

        private void SeedDatabase()
        {
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

            var companyAppUser = new ApplicationUser
            {
                Email = "payhawk@gmail.com",
                UserName = "payhawk@gmail.com"
            };

            var candidateAppUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre@gmail.com"
            };

            data.Users.Add(companyAppUser);
            data.Users.Add(candidateAppUser);

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
                SpecializationId = specialization.Id,
                CountryId = country.Id
            };

            companyId = company.Id;

            data.Companies.Add(company);

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
    }
}