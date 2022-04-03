namespace Internify.Web.Controllers
{
    using Services.Data.Message;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ChatController : Controller
    {
        private readonly IMessageService messageService;

        public ChatController(IMessageService messageService)
            => this.messageService = messageService;

        public IActionResult All()
        {
            var messages = messageService.GetLatestHundredMessages();

            return View(messages);
        }
    }
}