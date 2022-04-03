namespace Internify.Services.Data.Message
{
    using Ganss.XSS;
    using Internify.Data;
    using Internify.Data.Models;
    using Models.ViewModels.Message;

    public class MessageService : IMessageService
    {
        private readonly InternifyDbContext data;

        private const int LatestMessagesNumberConstant = 100;

        public MessageService(InternifyDbContext data)
            => this.data = data;

        public string Add(
            string senderEmail,
            string content)
        {
            var senderId = data
                .Users
                .FirstOrDefault(x =>
                x.Email == senderEmail
                && !x.IsDeleted)?.Id;

            if (senderId == null)
            {
                return null;
            }

            var sanitizer = new HtmlSanitizer();
            var sanitizedContent = sanitizer.Sanitize(content);

            var message = new Message
            {
                Content = sanitizedContent,
                SenderId = senderId
            };

            data.Messages.Add(message);
            data.SaveChanges();

            return sanitizedContent;
        }

        public IEnumerable<MessageListingViewModel> GetLatestHundredMessages()
            => data
            .Messages
            .OrderByDescending(x => x.SentOn)
            .Take(LatestMessagesNumberConstant)
            .Select(x => new MessageListingViewModel
            {
                Content = x.Content,
                SenderEmail = x.Sender.Email,
                SentOn = x.SentOn
            })
            .ToList();
    }
}