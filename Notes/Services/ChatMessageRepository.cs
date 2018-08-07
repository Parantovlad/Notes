using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Notes.Models;
using Notes.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Services
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly string _tableName;
        private readonly CloudTableClient _tableClient;
        private readonly NotesStorage _notesStorage;

        public ChatMessageRepository(IOptions<NotesStorage> notesStorage)
        {
            _notesStorage = notesStorage.Value ?? throw new ArgumentException(nameof(notesStorage)); ;

            var accountName = _notesStorage.AccountName;
            var accountKey = _notesStorage.AccountKey;
            _tableName = _notesStorage.TableName;

            var storageCredentials = new StorageCredentials(accountName, accountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            _tableClient = storageAccount.CreateCloudTableClient();
        }

        public async Task<IEnumerable<ChatMessage>> GetTopMessages(int number = 100)
        {
            var table = _tableClient.GetTableReference(_tableName);

            await table.CreateIfNotExistsAsync();

            string filter = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                "chatmessages");

            var query = new TableQuery<ChatMessageTableEntity>()
                .Where(filter)
                .Take(number);

            var entities = await table.ExecuteQuerySegmentedAsync(query, null);

            var result = entities.Results.Select(entity =>
                new ChatMessage
                {
                    Id = entity.RowKey,
                    Date = entity.Timestamp,
                    Message = entity.Message,
                    Sender = entity.Sender
                });

            return result;
        }

        public async Task AddMessage(ChatMessage message)
        {
            var table = _tableClient.GetTableReference(_tableName);

            await table.CreateIfNotExistsAsync();

            var chatMessage = new ChatMessageTableEntity(message.Id)
            {
                Message = message.Message,
                Sender = message.Sender
            };

            TableOperation insertOperation = TableOperation.Insert(chatMessage);

            var result = await table.ExecuteAsync(insertOperation);
        }
    }
}
