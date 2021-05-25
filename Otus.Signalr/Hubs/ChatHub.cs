using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Otus.Signalr.Models;
using Microsoft.AspNetCore.Authorization;
namespace Otus.Signalr.Hubs
{
    public interface IChatClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReceiveMessage(ChatMessage message);
    }

    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {

        public async Task SendMessage(ChatMessage m)
        {
            await Clients.All.ReceiveMessage(m);
        }
    }
}