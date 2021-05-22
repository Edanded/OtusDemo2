using System;

namespace Otus.Signalr.Models
{
    public class ChatMessage
    {
        public string Login{get;set;}
        public string Message {get;set;}
        public DateTime Timestamp{get;set;}
        public string To{get;set;}
    }
}