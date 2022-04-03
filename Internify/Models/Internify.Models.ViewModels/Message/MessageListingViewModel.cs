namespace Internify.Models.ViewModels.Message
{
    public class MessageListingViewModel
    {
        public string Content { get; init; }

        public string SenderEmail { get; init; }

        public DateTime SentOn { get; init; }
    }
}