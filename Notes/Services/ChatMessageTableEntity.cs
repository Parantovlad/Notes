using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Services
{
    public class ChatMessageTableEntity : TableEntity
    {
        public ChatMessageTableEntity(string key)
        {
            PartitionKey = "chatmessages";
            RowKey = key;
        }

        public ChatMessageTableEntity() { }

        public string Message { get; set; }

        public string Sender { get; set; }
    }
}
