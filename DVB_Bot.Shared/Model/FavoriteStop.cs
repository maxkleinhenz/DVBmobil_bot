using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Shared.Model
{
    public class FavoriteStop :TableEntity
    {
        public FavoriteStop(string chatId, string stopShortName)
        {
            this.ChatId = chatId;
            this.StopShortName = stopShortName;
        }

        public FavoriteStop(DynamicTableEntity tableEntity) : base(tableEntity.PartitionKey, tableEntity.RowKey)
        {

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
    }
}
