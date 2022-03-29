namespace Internify.Tests.Data.Services
{
    using Internify.Data;
    using Internify.Data.Models;
    using Internify.Services.Data.University;
    using Microsoft.EntityFrameworkCore;
    using Models.InputModels.University;
    using Models.ViewModels.University;
    using NUnit.Framework;
    using System.Linq;

    public class UniversityServiceTests
    {
        private InternifyDbContext data;
        private IUniversityService universityService;

        private string universityId;
        private string universityAppUserId;
        private string countryId;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            universityService = new UniversityService(data, null);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void IsUniversity_ShouldReturn_FalseWhenUniversityIdDoesNotMatch()
        {
            var actualResult = universityService.IsUniversity("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsUniversity_ShouldReturn_FalseWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.IsUniversity(universityId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsUniversity_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = universityService.IsUniversity(universityId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsUniversityByUserId_ShouldReturn_FalseWhenUserIdDoesNotMatch()
        {
            var actualResult = universityService.IsUniversityByUserId("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsUniversityByUserId_ShouldReturn_FalseWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.IsUniversityByUserId(universityAppUserId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsUniversityByUserId_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = universityService.IsUniversityByUserId(universityAppUserId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_NullWhenUniversityIdDoesNotMatch()
        {
            var actualResult = universityService.GetIdByUserId("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_NullWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.GetIdByUserId(universityAppUserId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetIdByUserId_ShouldReturn_UniversityId()
        {
            SeedDatabase();

            var actualResult = universityService.GetIdByUserId(universityAppUserId);

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenUserIdDoesNotMatch()
        {
            var actualResult = universityService.Add(
                "fwefw",
                "MIT",
                "somedomain.com/avatar.jpg",
                "somedomain.com",
                1885,
                Internify.Data.Models.Enums.Type.Private,
                "fpwoekfwopef",
                countryId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_NullWhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = universityService.Add(
                universityAppUserId,
                "MIT",
                "somedomain.com/avatar.jpg",
                "somedomain.com",
                1885,
                Internify.Data.Models.Enums.Type.Private,
                "fpwoekfwopef",
                "238rj023fj03");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = universityService.Add(
                universityAppUserId,
                "MIT",
                "somedomain.com/avatar.jpg",
                "somedomain.com",
                1885,
                Internify.Data.Models.Enums.Type.Private,
                "fpwoekfwopef",
                countryId);

            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenUniversityIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = universityService.Edit(
                "",
                "new name",
                "ffff.com/avatar.jpg",
                "ffff.com",
                2022,
                Internify.Data.Models.Enums.Type.Public,
                "fwpekfwope",
                countryId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.Edit(
                universityId,
                "new name",
                "ffff.com/avatar.jpg",
                "ffff.com",
                2022,
                Internify.Data.Models.Enums.Type.Public,
                "fwpekfwope",
                countryId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldReturn_FalseWhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = universityService.Edit(
                universityId,
                "new name",
                "ffff.com/avatar.jpg",
                "ffff.com",
                2022,
                Internify.Data.Models.Enums.Type.Public,
                "fwpekfwope",
                "f093j3f029");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Edit_ShouldChange_ModifiedOnProperty_AndReturn_True()
        {
            SeedDatabase();

            var actualResult = universityService.Edit(
                universityId,
                "new name",
                "ffff.com/avatar.jpg",
                "ffff.com",
                2022,
                Internify.Data.Models.Enums.Type.Public,
                "f-23fk2-3fk",
                countryId);

            var actualModifiedOnProperty = data.Universities.Find(universityId)?.ModifiedOn;

            Assert.IsTrue(actualResult);
            Assert.IsNotNull(actualModifiedOnProperty);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenUniversityIdDoesNotMatch()
        {
            var actualResult = universityService.Delete("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_FalseWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.Delete(universityId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_ShouldReturn_True()
        {
            SeedDatabase();

            var actualDeleteResult = universityService.Delete(universityId);
            var actualExistsResult = universityService.Exists(universityId);

            Assert.IsTrue(actualDeleteResult);
            Assert.IsFalse(actualExistsResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenUniversityIdDoesNotMatch()
        {
            var actualResult = universityService.Exists("");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_FalseWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.Exists(universityId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            SeedDatabase();

            var actualResult = universityService.Exists(universityId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenUniversityIdDoesNotMatch()
        {
            var actualResult = universityService.GetDetailsModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_NullWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.GetDetailsModel(universityId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetDetailsModel_ShouldReturn_UniversityDetailsViewModel()
        {
            SeedDatabase();

            var expectedModel = new UniversityDetailsViewModel
            {
                Id = universityId,
                Name = "Software University",
                ImageUrl = "www.somedomain.com/img.jpg",
                WebsiteUrl = "www.somedomain.com",
                Founded = 2013,
                Type = "Private",
                Description = "Quality education at an affordable price.",
                Country = "Bulgaria"
            };

            var actualModel = universityService.GetDetailsModel(universityId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Name").EqualTo(expectedModel.Name)
                                   & Has.Property("ImageUrl").EqualTo(expectedModel.ImageUrl)
                                   & Has.Property("WebsiteUrl").EqualTo(expectedModel.WebsiteUrl)
                                   & Has.Property("Founded").EqualTo(expectedModel.Founded)
                                   & Has.Property("Type").EqualTo(expectedModel.Type)
                                   & Has.Property("Description").EqualTo(expectedModel.Description)
                                   & Has.Property("Country").EqualTo(expectedModel.Country));

            Assert.IsFalse(actualModel.HasAlumni);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenUniversityIdDoesNotMatch()
        {
            var actualResult = universityService.GetEditModel("");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_NullWhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.GetEditModel(universityId);

            Assert.IsNull(actualResult);
        }

        [Test]
        public void GetEditModel_ShouldReturn_EditUniversityFormModel()
        {
            SeedDatabase();

            var expectedModel = new EditUniversityFormModel
            {
                Id = universityId,
                Name = "Software University",
                ImageUrl = "www.somedomain.com/img.jpg",
                WebsiteUrl = "www.somedomain.com",
                Founded = 2013,
                Type = Internify.Data.Models.Enums.Type.Private,
                Description = "Quality education at an affordable price.",
                CountryId = countryId
            };

            var actualModel = universityService.GetEditModel(universityId);

            Assert.That(actualModel, Has.Property("Id").EqualTo(expectedModel.Id)
                                   & Has.Property("Name").EqualTo(expectedModel.Name)
                                   & Has.Property("ImageUrl").EqualTo(expectedModel.ImageUrl)
                                   & Has.Property("WebsiteUrl").EqualTo(expectedModel.WebsiteUrl)
                                   & Has.Property("Founded").EqualTo(expectedModel.Founded)
                                   & Has.Property("Type").EqualTo(expectedModel.Type)
                                   & Has.Property("Description").EqualTo(expectedModel.Description)
                                   & Has.Property("CountryId").EqualTo(expectedModel.CountryId));
        }

        [Test]
        public void All_ShouldReturn_UniversityListingQueryModel_WithEmptyUniversitiesCollection_WhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = universityService.All(
                null,
                null,
                "fwefe",
                1,
                6);

            Assert.IsEmpty(actualResult.Universities);
        }

        [Test]
        public void All_ShouldReturn_UniversityListingQueryModel_WithEmptyUniversitiesCollection_WhenAllUniversitiesAreDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.All(
                null,
                null,
                "fwefe",
                1,
                6);

            Assert.IsEmpty(actualResult.Universities);
        }

        [Test]
        public void All_ShouldSanitizeName_AndReturn_UniversityListingQueryModel_WithFilteredUniversitiesCollection()
        {
            SeedDatabase();

            var actualResult = universityService.All(
                "software",
                null,
                null,
                1,
                6);

            Assert.That(actualResult.Universities.Count(), Is.EqualTo(1));
        }

        [Test]
        public void All_ShouldReturn_UniversityListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            SeedDatabase();

            var actualResultOne = universityService.All(
                "software",
                null,
                "fwefe",
                -1,
                -1);

            var actualResultTwo = universityService.All(
                "software",
                null,
                "fwefe",
                0,
                999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.UniversitiesPerPage, Is.EqualTo(6));
            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.UniversitiesPerPage, Is.EqualTo(96));
        }

        [Test]
        public void Alumni_ShouldReturn_CandidateListingQueryModel_WithEmptyCandidatesCollection_WhenUniversityIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = universityService.Alumni(
                "fwef",
                null,
                null,
                null,
                null,
                false,
                1,
                6);

            Assert.IsEmpty(actualResult.Candidates);
        }

        [Test]
        public void Alumni_ShouldReturn_CandidateListingQueryModel_WithEmptyCandidatesCollection_WhenUniversityIsDeleted()
        {
            SeedDatabase();

            universityService.Delete(universityId);

            var actualResult = universityService.Alumni(
                universityId,
                null,
                null,
                null,
                null,
                false,
                1,
                6);

            Assert.IsEmpty(actualResult.Candidates);
        }

        [Test]
        public void Alumni_ShouldReturn_CandidateListingQueryModel_WithEmptyCandidatesCollection_WhenSpecializationIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = universityService.Alumni(
                universityId,
                null,
                null,
                "fwefw",
                null,
                false,
                1,
                6);

            Assert.IsEmpty(actualResult.Candidates);
        }

        [Test]
        public void Alumni_ShouldReturn_CandidateListingQueryModel_WithEmptyCandidatesCollection_WhenCountryIdDoesNotMatch()
        {
            SeedDatabase();

            var actualResult = universityService.Alumni(
                universityId,
                null,
                null,
                null,
                "fweopfim",
                false,
                1,
                6);

            Assert.IsEmpty(actualResult.Candidates);
        }

        [Test]
        public void Alumni_ShouldReturn_CandidateListingQueryModel_WithCorrectPageAndPerPageProperties()
        {
            SeedDatabase();

            var actualResultOne = universityService.Alumni(
                universityId,
                null,
                null,
                null,
                null,
                false,
                -1,
                -1);

            var actualResultTwo = universityService.Alumni(
                universityId,
                null,
                null,
                null,
                null,
                false,
                0,
                999);

            Assert.That(actualResultOne.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultOne.CandidatesPerPage, Is.EqualTo(6));
            Assert.That(actualResultTwo.CurrentPage, Is.EqualTo(1));
            Assert.That(actualResultTwo.CandidatesPerPage, Is.EqualTo(96));
        }

        private void SeedDatabase()
        {
            var country = new Country
            {
                Name = "Bulgaria"
            };

            countryId = country.Id;

            data.Countries.Add(country);

            var universityAppUser = new ApplicationUser
            {
                Email = "softuni@gmail.com",
                UserName = "softuni@gmail.com"
            };

            universityAppUserId = universityAppUser.Id;

            data.Users.Add(universityAppUser);

            data.SaveChanges();

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

            data.SaveChanges();
        }
    }
}