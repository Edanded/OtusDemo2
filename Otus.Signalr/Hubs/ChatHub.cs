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
            if (string.IsNullOrWhiteSpace(m.To))
            {
                await Clients.All.ReceiveMessage(m);
            }
            else
            {
                await Clients.User(m.To).ReceiveMessage(m);
                await Clients.User(m.Login).ReceiveMessage(m);
            }
        }
    }


    public class SignalUserProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var user = connection.GetHttpContext().User?.Identity?.Name;
            return user;
        }
    }

}