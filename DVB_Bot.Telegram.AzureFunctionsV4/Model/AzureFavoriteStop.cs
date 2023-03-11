using System;
using System.Diagnostics;
using Azure;
using Azure.Data.Tables;
using DVB_Bot.Shared.Model;

namespace DVB_Bot.Telegram.AzureFunctionsV4.Model
{
    [DebuggerDisplay("{ChatId} {StopShortName}")]
    public class AzureFavoriteStop : ITableEntity, IFavoriteStop
    {
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        /// <summary>
        /// PartitionKey
        /// </summary>
        public string? ChatId
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        /// <summary>
        /// RowKey
        /// </summary>
        public string? StopShortName
        {
            get => RowKey;
            set => RowKey = value;
        }
        public DateTime? AddDateTime { get; set; }

        public AzureFavoriteStop() { }

        public AzureFavoriteStop(string chatId, string stopShortName, DateTime? addDateTime = null)
        {
            PartitionKey = chatId;
            RowKey = stopShortName.ToUpper();
            AddDateTime = addDateTime ?? DateTime.UtcNow;
        }
    }
}
