namespace Internify.Services.Data.Tests
{
    using Internify.Data;
    using Internify.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using Specialization;
    using System.Linq;

    public class SpecializationServiceTests
    {
        private InternifyDbContext data;
        private ISpecializationService specializationService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            specializationService = new SpecializationService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void All_ShouldReturn_EmptyCollection()
        {
            var actualResult = specializationService.All();

            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void All_ShouldReturn_SpecializationListingViewModel()
        {
            var specialization = new Specialization
            {
                Name = "Software Development"
            };

            data.Specializations.Add(specialization);
            data.SaveChanges();

            var actualFirstSpecializationName = specializationService
                .All()
                .FirstOrDefault()?.Name;

            Assert.That(actualFirstSpecializationName, Is.EqualTo(specialization.Name));
        }

        [Test]
        public void Exists_ShouldReturn_False()
        {
            var actualResult = specializationService.Exists("fwefw");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            var specialization = new Specialization
            {
                Name = "Software Development"
            };

            data.Specializations.Add(specialization);
            data.SaveChanges();

            var actualResult = specializationService.Exists(specialization.Id);

            Assert.IsTrue(actualResult);
        }
    }
}