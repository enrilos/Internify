namespace Internify.Tests.Services
{
    using Data;
    using Data.Models;
    using Internify.Services.Article;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.Article;
    using Models.ViewModels.Article;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    public class ArticleServiceTests
    {
        private InternifyDbContext data;
        private IArticleService articleService;

        private string articleId;
        private string companyId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            articleService = new ArticleService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ShouldReturn_NullWhenCompanyIdDoesNotMatch()
        {
            var actualResult = articleService.Add(
                "",
                "Fake News",
                null,
                "Extra! Extra! Real All About It!",
                "somecooldomain.com");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_ArticleId()
        {
            SeedDatabase();

            var actualResult = articleService.Add(
                companyId,
                "Fake News",
                null,
                "Extra! Extra! Real All About It!",
                "somecooldomain.com");

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenArticleIdDoesNotMatch()
        {
            var actualResult = articleService.Edit(
                "ef23f23f",
                "some title",
                "some content");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenArticleIsDeleted()
        {
            SeedDatabase();

            articleService.Delete(articleId);

            var actualResult = articleService.Edit(
                articleId,
                "some title",
                "some content");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldChange_ModifiedOnProperty_AndReturn_ArticleIdWhenSuccessful()
        {
            SeedDatabase();

            var actualResult = articleService.Edit(
                articleId,
                "some title",
                "some content");

            var actualModifiedOnProperty = articleService.GetDetailsModel(articleId)?.ModifiedOn;

            Assert.IsTrue(actualResult);
            Assert.IsNotNull(actualModifiedOnProperty);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenArticleIdDoesNotMatch()
        {
            var actualResult = articleService.Delete("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenArticleIsDeleted()
        {
            SeedDatabase();

            articleService.Delete(articleId);

            var actualResult = articleService.Delete(articleId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_True()
        {
            SeedDatabase();

            var actualDeleteResult = articleService.Delete(articleId);
            var actualExistsResult = articleService.Exists(articleId);

            Assert.IsTrue(actualDeleteResult);
            Assert.IsFalse(actualExistsResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenArticleIdDoesNotMatch()
        {
            var actualResult = articleService.Exists("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenArticleIsDeleted()
        {
            SeedDatabase();

            articleService.Delete(articleId);

            var actualResult = articleService.Exists(articleId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = articleService.Exists(articleId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenArticleIdDoesNotMatch()
        {
            var actualResult = articleService.GetDetailsModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenArticleIsDeleted()
        {
            SeedDatabase();

            articleService.Delete(articleId);

            var actualResult = articleService.GetDetailsModel(articleId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldHave_ModifiedOnPropertyNotNullAfterEdit()
        {
            SeedDatabase();

            articleService.Edit(
                articleId,
                "some title",
                "some content");

            var actualModifiedOn = articleService.GetDetailsModel(articleId)?.ModifiedOn;

            Assert.IsNotNull(actualModifiedOn);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_ArticleDetailsViewModel()
        {
            SeedDatabase();

            var expectedModel = new ArticleDetailsViewModel
            {
                Id = articleId,
                Title = "Fake News",
                ImageUrl = null,
                Content = "Extra! Extra! Real All About It!",
                CompanyId = companyId,
                CompanyName = "Payhawk"
            };

            var actualModel = articleService.GetDetailsModel(articleId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Title").EqualTo(expectedModel.Title)
                                   & Has.Property("ImageUrl").Null
                                   & Has.Property("Content").EqualTo(expectedModel.Content)
                                   & Has.Property("CompanyId").EqualTo(expectedModel.CompanyId)
                                   & Has.Property("CompanyName").EqualTo(expectedModel.CompanyName));

            Assert.IsNotNull(actualModel.CreatedOn);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenArticleIdDoesNotMatch()
        {
            var actualResult = articleService.GetEditModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenArticleIsDeleted()
        {
            SeedDatabase();

            articleService.Delete(articleId);

            var actualResult = articleService.GetEditModel(articleId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_EditArticleFormModel()
        {
            SeedDatabase();

            var expectedModel = new EditArticleFormModel
            {
                Id = articleId,
                Title = "Fake News",
                Content = "Extra! Extra! Real All About It!",
            };

            var actualModel = articleService.GetEditModel(articleId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Title").EqualTo(expectedModel.Title)
                                   & Has.Property("Content").EqualTo(expectedModel.Content));
        }

        [Test]
        public void IsOwnedByCompany_ShouldReturn_FalseWhenArticleIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = articleService.IsOwnedByCompany(
                "",
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsOwnedByCompany_ShouldReturn_FalseWhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = articleService.IsOwnedByCompany(
                articleId,
                "");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsOwnedByCompany_ShouldReturn_FalseWhenArticleIsDeleted()
        {
            SeedDatabase();

            articleService.Delete(articleId);

            var actualResult = articleService.IsOwnedByCompany(
                articleId,
                companyId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsOwnedByCompany_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = articleService.IsOwnedByCompany(
                articleId,
                companyId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetCompanyArticles_ShouldReturn_ArticleListingQueryModel_WithEmptyArticlesCollection_WhenCompanyIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = articleService.GetCompanyArticles(
                "",
                "Payhawk",
                null,
                1,
                5);

            Assert.IsEmpty(actualResult.Articles);
        }

        [Test]
        public void GetCompanyArticles_ShouldReturn_ArticleListingQueryModel_WithEmptyArticlesCollection_WhenCompanyIsDeleted()
        {
            SeedDatabase();

            articleService.Delete(articleId);

            var actualResult = articleService.GetCompanyArticles(
                companyId,
                "Payhawk",
                null,
                1,
                5);

            Assert.IsEmpty(actualResult.Articles);
        }

        [Test]
        public void GetCompanyArticles_ShouldReturn_ArticleListingQueryModel_WithFetchedCompanyName_WhenItIsNullOrEmpty()
        {
            SeedDatabase();

            var actualResult = articleService.GetCompanyArticles(
                companyId,
                null,
                null,
                1,
                5);

            Assert.That(actualResult.CompanyName, Is.EqualTo("Payhawk"));
        }

        [Test]
        public void GetCompanyArticles_ShouldSanitizeTitle_AndReturn_ArticleListingQueryModel_WithFilteredArticlesCollection()
        {
            SeedDatabase();

            var articles = new List<Article>()
            {
                new Article
                {
                    CompanyId = companyId,
                    Title = "Some Article",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "How to improve",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "Things to know about React's useEffect() hook",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "Start your Web API project with ease",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "Are you aware of null bytes (\\0) ?",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "How does the C# GC work and what are boxing and unboxing ?",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                }
            };

            data.Articles.AddRange(articles);

            data.SaveChanges();

            var actualResultOne = articleService.GetCompanyArticles(
                companyId,
                "<script>Payhawk</script> some nonexistent company",
                "<script>This script tags and the content will be removed</script> some nonexistent article(s)",
                1,
                5);

            var actualResultTwo = articleService.GetCompanyArticles(
                companyId,
                "Payhawk",
                "a",
                1,
                12);

            var actualResultThree = articleService.GetCompanyArticles(
                companyId,
                "Payhawk",
                "c#",
                1,
                5);

            Assert.IsEmpty(actualResultOne.Articles);
            Assert.That(actualResultTwo.Articles.Count(), Is.EqualTo(6));
            Assert.That(actualResultThree.Articles.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetCompanyArticles_ShouldReturn_ArticleListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            SeedDatabase();

            var articles = new List<Article>()
            {
                new Article
                {
                    CompanyId = companyId,
                    Title = "Some Article",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "How to improve",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "Things to know about React's useEffect() hook",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "Start your Web API project with ease",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "Are you aware of null bytes (\\0) ?",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                },
                new Article
                {
                    CompanyId = companyId,
                    Title = "How does the C# GC work and what are boxing and unboxing ?",
                    ImageUrl = null,
                    Content = "Extra! Extra! Real All About It"
                }
            };

            data.Articles.AddRange(articles);

            data.SaveChanges();

            var actualResultOne = articleService.GetCompanyArticles(
                companyId,
                "PayHawk",
                "a",
                -1,
                -1);

            var actualResultTwo = articleService.GetCompanyArticles(
               companyId,
               "Payhawk",
               "a",
               1,
               4);

            var actualResultThree = articleService.GetCompanyArticles(
                companyId,
                "PayHawk",
                "",
                -111,
                999);

            Assert.That(actualResultOne.Articles.Count(), Is.EqualTo(6));
            Assert.That(actualResultTwo.Articles.Count(), Is.EqualTo(6));
            Assert.That(actualResultThree.Articles.Count(), Is.EqualTo(7));
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

            data.SaveChanges();

            // Add Identity User
            var companyAppUser = new ApplicationUser
            {
                Email = "payhawk@gmail.com",
                UserName = "payhawk123"
            };

            data.Users.Add(companyAppUser);

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

            data.SaveChanges();

            var article = new Article
            {
                CompanyId = company.Id,
                Title = "Fake News",
                ImageUrl = null,
                Content = "Extra! Extra! Real All About It!"
            };

            articleId = article.Id;

            data.Articles.Add(article);

            data.SaveChanges();
        }
    }
}