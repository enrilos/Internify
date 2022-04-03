namespace Internify.Services.Data.Message
{
    using Models.ViewModels.Message;

    public interface IMessageService
    {
        string Add(
            string senderEmail,
            string content);

        IEnumerable<MessageListingViewModel> GetLatestHundredMessages();
    }
}