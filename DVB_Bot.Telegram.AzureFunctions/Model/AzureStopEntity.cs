using DVB_Bot.Shared.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Telegram.AzureFunctions.Model
{
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

        public AzureStopEntity()
        {

        }

        public AzureStopEntity(string shortName) : base(shortName[0].ToString(), shortName)
        {

        }
    }
}
