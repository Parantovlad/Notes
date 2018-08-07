using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Services
{
    public interface IChatMessageRepository
    {
        Task AddMessage(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetTopMessages(int number = 100);
    }
}
