namespace Internify.Web.Hubs
{
    using Services.Data.Message;
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;

        public ChatHub(IMessageService messageService)
            => this.messageService = messageService;

        public async Task SendMessage(string user, string message)
        {
            var sanitizedMessage = messageService.Add(user, message);

            if (sanitizedMessage == null)
            {
                return;
            }

            await Clients.All.SendAsync("ReceiveMessage", user, sanitizedMessage);
        }
    }
}