namespace Internify.Services.Data.Tests
{
    using Internify.Data;
    using Internify.Data.Models;
    using Message;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    public class MessageServiceTests
    {
        private InternifyDbContext data;
        private IMessageService messageService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InternifyDbContext>()
                .UseInMemoryDatabase("MyInMemoryDatabase")
                .Options;

            data = new InternifyDbContext(options);
            data.Database.EnsureCreated();
            messageService = new MessageService(data);
        }

        [TearDown]
        public void TearDown()
        {
            data.Database.EnsureDeleted();
        }

        [Test]
        public void Add_ShouldReturn_NullWhenSenderEmailDoesNotMatch()
        {
            var actualResult = messageService.Add(
                "fpwekfowpef@somewef.ef23",
                "hello this is message");

            Assert.IsNull(actualResult);
        }

        [Test]
        public void Add_ShouldReturn_SanitizedMessage()
        {
            var expectedResult = "hello";

            var appUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre@gmail.com"
            };

            data.Users.Add(appUser);
            data.SaveChanges();

            var actualResult = messageService.Add(
                "emre@gmail.com",
                "<script>hello</script>hello");

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetLatestHundredMessages_ShouldReturn_EmptyCollection()
        {
            var actualResult = messageService.GetLatestHundredMessages();

            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void GetLatestHundredMessages_ShouldReturn_CollectionOfType_MessageListingViewModel()
        {
            var appUser = new ApplicationUser
            {
                Email = "emre@gmail.com",
                UserName = "emre@gmail.com"
            };

            data.Users.Add(appUser);

            var message = new Message
            {
                SenderId = appUser.Id,
                Content = "hello"
            };

            data.Messages.Add(message);
            data.SaveChanges();

            var actualResult = messageService.GetLatestHundredMessages();

            Assert.That(actualResult.Count(), Is.EqualTo(1));
        }
    }
}