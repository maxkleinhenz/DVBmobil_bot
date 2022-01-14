using System.Diagnostics;
using DVB_Bot.Shared.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Telegram.AzureFunctions.Model
{
    [DebuggerDisplay("{ShortName} {Name}")]
    public class AzureStopEntity : TableEntity, IStop
    {
        /// <summary>
        /// RowKey
        /// </summary>
        public string ShortName
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string City { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UrlStop { get; set; }
        public string UrlDeparture { get; set; }

        public AzureStopEntity() { }

        public AzureStopEntity(string shortName) : base(shortName[0].ToString().ToUpper(), shortName.ToUpper()) { }

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
