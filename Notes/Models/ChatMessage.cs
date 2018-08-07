using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class ChatMessage
    {
        public ChatMessage(Guid id)
        {
            Id = id.ToString("X");
            Date = DateTimeOffset.Now;
        }
        public ChatMessage() { }
        public string Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
    }
}
