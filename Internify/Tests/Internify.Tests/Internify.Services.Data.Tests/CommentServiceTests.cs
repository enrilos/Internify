namespace Internify.Services.Data.Tests
{
    using Comment;
    using Internify.Data;
    using Internify.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommentServiceTests
    {
        private InternifyDbContext data;
        private ICommentService commentService;

        private string articleId;
        private string candidateId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            commentService = new CommentService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void CommentArticle_ShouldReturn_NullWhenArticleIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = commentService.CommentArticle(
                "fewf",
                candidateId,
                "some cool comment");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void CommentArticle_ShouldReturn_NullWhenArticleIsDeleted()
        {
            SeedDatabase();

            var article = data.Articles.Find(articleId);
            data.Articles.Remove(article);
            data.SaveChanges();

            var actualResult = commentService.CommentArticle(
                articleId,
                candidateId,
                "sone coooool comment");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void CommentArticle_ShouldReturn_NullWhenCandidateIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = commentService.CommentArticle(
                articleId,
                "fwef",
                "some cool comment");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void CommentArticle_ShouldReturn_NullWhenCandidateIsDeleted()
        {
            SeedDatabase();

            var candidate = data.Candidates.Find(candidateId);
            data.Candidates.Remove(candidate);
            data.SaveChanges();

            var actualResult = commentService.CommentArticle(
                articleId,
                candidateId,
                "sone coooool comment");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void CommentArticle_ShouldReturn_CommentId()
        {
            SeedDatabase();

            var actualResult = commentService.CommentArticle(
                articleId,
                candidateId,
                "sone coooool comment");

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void ArticleComments_ShouldReturn_CommentListingQueryModel_WithEmptyCommentsCollection_WhenArticleIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = commentService.ArticleComments(
                "fwef",
                1,
                6);

            Assert.IsEmpty(actualResult.Comments);
        }

        [Test]
        public void ArticleComments_ShouldReturn_CommentListingQueryModel_WithEmptyCommentsCollection_WhenArticleIsDeleted()
        {
            SeedDatabase();

            var article = data.Articles.Find(articleId);
            data.Articles.Remove(article);
            data.SaveChanges();

            var actualResult = commentService.ArticleComments(
                articleId,
                1,
                6);

            Assert.IsEmpty(actualResult.Comments);
        }

        [Test]
        public void ArticleComments_ShouldReturn_CommentListingQueryModel_WithFilteredCommentsBasedOnArticleId()
        {
            SeedDatabase();

            var actualResult = commentService.ArticleComments(
                articleId,
                1,
                6);

            Assert.That(actualResult.Comments.Count(), Is.EqualTo(6));
        }

        [Test]
        public void ArticleComments_ShouldReturn_CommentListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            SeedDatabase();

            var actualResult = commentService.ArticleComments(
               articleId,
              -1,
              -5);

            var actualResultTwo = commentService.ArticleComments(
               articleId,
              0,
              150);

            Assert.That(actualResult.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResult.CommentsPerPage, Is.EqualTo(6));
            Assert.That(actualResult.Comments.Count(), Is.EqualTo(6));

            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.CommentsPerPage, Is.EqualTo(96));
            Assert.That(actualResultTwo.Comments.Count(), Is.EqualTo(6));
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

            data.Companies.Add(company);

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

            var article = new Article
            {
                Title = "Some cool article here",
                ImageUrl = null,
                Content = "Extra! Extra! Read all about it Extra! Extra! Read all about it",
                CompanyId = company.Id
            };

            articleId = article.Id;

            data.Articles.Add(article);
            data.SaveChanges();

            var comments = new List<Comment>()
            {
                new Comment
                {
                    CandidateId = candidate.Id,
                    Content = "Amazing article. Bravo! Bravooo!",
                    ArticleId = article.Id
                },
                new Comment
                {
                    CandidateId = candidate.Id,
                    Content = "fwefwefefwefwefwef",
                    ArticleId = article.Id
                },
                new Comment
                {
                    CandidateId = candidate.Id,
                    Content = "some coooooooooooooooool content here",
                    ArticleId = article.Id
                },
                new Comment
                {
                    CandidateId = candidate.Id,
                    Content = "01010101010010101010",
                    ArticleId = article.Id
                },
                new Comment
                {
                    CandidateId = candidate.Id,
                    Content = "Can you say the same? Huh?",
                    ArticleId = article.Id
                },
                new Comment
                {
                    CandidateId = candidate.Id,
                    Content = "blablablablblalbalablabl",
                    ArticleId = article.Id
                }
            };

            data.Comments.AddRange(comments);
            data.SaveChanges();
        }
    }
}