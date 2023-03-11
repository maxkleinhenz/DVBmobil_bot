using System;
using System.Diagnostics;
using Azure;
using Azure.Data.Tables;
using DVB_Bot.Shared.Model;

namespace DVB_Bot.Telegram.AzureFunctionsV4.Model
{
    [DebuggerDisplay("{ShortName} {Name}")]
    public class AzureStopEntity : ITableEntity, IStop
    {
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        /// <summary>
        /// RowKey
        /// </summary>
        public string ShortName
        {
            get => RowKey;
            set
            {
                PartitionKey= value[0].ToString().ToUpper();
                RowKey = value.ToUpper();
            }
        }

        public string? City { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? UrlStop { get; set; }
        public string? UrlDeparture { get; set; }
        
        public AzureStopEntity() { }

        public AzureStopEntity(string shortName)
        {
            ShortName = shortName;
        }

        public AzureStopEntity(IStop stop) : this(stop.ShortName)
        {
            ShortName = stop.ShortName.ToUpper();
            City = stop.City;
            Name = stop.Name;
            Code = stop.Code;
            UrlStop = stop.UrlStop;
            UrlDeparture = stop.UrlDeparture;
        }
    }
}
