using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Services
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessage>> GetAllInitially();
        Task<ChatMessage> CreateNewMessage(string senderName, string message);
    }
}
