using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Otus.Signalr.Hubs;
using Otus.Signalr.Models;
using Otus.Signalr.Requests;

namespace Otus.Signalr.Controllers
{
    [ApiController]
    [Route("/api/ccc")]
    public class ChatController:ControllerBase
    {
         private readonly IHubContext<ChatHub, IChatClient> _chatHub;

        public ChatController(IHubContext<ChatHub, IChatClient> chatHub)
        {
            _chatHub = chatHub;
        }


        [HttpPost("login")]
        public void Login([FromBody] LoginRequest request)
        {

        }

        [HttpPost("send")]
        public void SendMEssage([FromBody]ChatMessage message){

        }

        [HttpGet]
        public string Foo()
        =>"af";

    }
}