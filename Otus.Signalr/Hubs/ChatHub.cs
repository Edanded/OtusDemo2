using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Otus.Signalr.Models;

namespace Otus.Signalr.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
    // public class ChatHub : Hub<IChatClient>
    // {

    //     public async Task SendMessage(ChatMessage m)
    //     {

    //         var user=m.To;
    //         var message=m.Message;
    //         if (string.IsNullOrWhiteSpace(user))
    //         {
    //             Console.WriteLine("fafafa");
    //             await Clients.All.ReceiveMessage( m);
    //         }
    //         else
    //         {
    //             await Clients.User(user).ReceiveMessage(m);
    //         }

    //     }
    // }






    public class ChatHub : Hub
    {
        public async Task SendMessage(ChatMessage m)
        {
            var user = m.To;
            var message = m.Message;
            if (string.IsNullOrWhiteSpace(user))
            {
                await Clients.All.SendAsync("ReceiveMessage", m);
            }
            else
            {
                await Clients.User(user).SendAsync("ReceiveMessage", m);
            }

        }
    }
}