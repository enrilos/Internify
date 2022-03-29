namespace Internify.Tests.Data.Services
{
    using Internify.Data;
    using Internify.Data.Models;
    using Internify.Services.Data.Country;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Linq;

    public class CountryServiceTests
    {
        private InternifyDbContext data;
        private ICountryService countryService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            countryService = new CountryService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void All_ShouldReturn_EmptyCollection()
        {
            var actualResult = countryService.All();

            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void All_ShouldReturn_CountryListingViewModel()
        {
            var country = new Country
            {
                Name = "Mordor"
            };

            data.Countries.Add(country);
            data.SaveChanges();

            var actualFirstCountryName = countryService
                .All()
                .FirstOrDefault()?.Name;

            Assert.That(actualFirstCountryName, Is.EqualTo(country.Name));
        }

        [Test]
        public void Exists_ShouldReturn_False()
        {
            var actualResult = countryService.Exists("fwefw");

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Exists_ShouldReturn_True()
        {
            var country = new Country
            {
                Name = "Mordor"
            };

            data.Countries.Add(country);
            data.SaveChanges();

            var actualResult = countryService.Exists(country.Id);

            Assert.IsTrue(actualResult);
        }
    }
}