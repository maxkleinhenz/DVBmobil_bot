using System;
using System.Diagnostics;
using DVB_Bot.Shared.Model;
using Microsoft.Azure.Cosmos.Table;

namespace DVB_Bot.Telegram.AzureFunctions.Model
{
    [DebuggerDisplay("{ChatId} {StopShortName}")]
    public class AzureFavoriteStop : TableEntity, IFavoriteStop
    {
        /// <summary>
        /// PartitionKey
        /// </summary>
        public string ChatId { get; set; }

        /// <summary>
        /// RowKey
        /// </summary>
        public string StopShortName { get; set; }
        public DateTime AddDateTime { get; set; }

        public AzureFavoriteStop() { }

        public AzureFavoriteStop(DynamicTableEntity tableEntity) : base(tableEntity.PartitionKey, tableEntity.RowKey)
        {
            tableEntity.Properties.TryGetValue(nameof(IFavoriteStop.AddDateTime), out var property);
            AddDateTime = (property?.DateTime).GetValueOrDefault();
        }

        public AzureFavoriteStop(string chatId, string stopShortName, DateTime? addDateTime = null)
        {
            this.ChatId = chatId;
            this.StopShortName = stopShortName.ToUpper();
            AddDateTime = addDateTime ?? DateTime.Now;
        }
    }
}
