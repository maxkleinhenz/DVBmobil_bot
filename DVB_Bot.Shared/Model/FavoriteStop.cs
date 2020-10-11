using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Shared.Model
{
    public class FavoriteStop :TableEntity
    {
        public FavoriteStop(string chatId, string stopShortName)
        {
            this.ChatId = chatId;
            this.StopShortName = stopShortName;
            AddDateTime = DateTime.Now;
        }

        public FavoriteStop(DynamicTableEntity tableEntity) : base(tableEntity.PartitionKey, tableEntity.RowKey)
        {
            tableEntity.Properties.TryGetValue(nameof(FavoriteStop.AddDateTime), out var property);
            AddDateTime = (property?.DateTime).GetValueOrDefault();
        }

        public FavoriteStop()
        {
            
        }

        public string ChatId
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public string StopShortName {
            get => RowKey;
            set => RowKey = value;
        }

        public DateTime AddDateTime { get; set; }
    }
}
