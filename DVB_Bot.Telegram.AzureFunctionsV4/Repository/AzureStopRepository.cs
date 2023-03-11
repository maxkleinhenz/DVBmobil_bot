using Azure.Data.Tables;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Repository;
using DVB_Bot.Telegram.AzureFunctionsV4.Model;

namespace DVB_Bot.Telegram.AzureFunctionsV4.Repository
{
    public class AzureStopRepository : IStopRepository
    {
        private readonly TableClientHelper tableClientHelper;

        public AzureStopRepository(TableClient tableClient)
        {
            tableClientHelper = new TableClientHelper(tableClient);
        }

        public async Task<IStop> GetStopByShortNameAsync(string shortName)
        {
            var stop = new AzureStopEntity(shortName);
            var stopEntity = await tableClientHelper.QueryEntityAsync<AzureStopEntity>(stop.PartitionKey, stop.RowKey);
            return stopEntity;
        }

        public async Task<List<IStop>> GetAllStopsAsync()
        {
            var stops = await tableClientHelper.QueryAllAsync<AzureStopEntity>();
            return new List<IStop>(stops);
        }
    }
}
