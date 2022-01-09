using System.Collections.Generic;
using System.Threading.Tasks;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Repository;
using DVB_Bot.Telegram.AzureFunctions.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Telegram.AzureFunctions.Repository
{
    public class AzureStopRepository : IStopRepository
    {
        private readonly CloudTableHelper _cloudTableHelper;

        public AzureStopRepository(CloudTable table)
        {
            _cloudTableHelper = new CloudTableHelper(table);
        }

        public async Task<IStop> GetStopByShortNameAsync(string shortName)
        {
            var stopEntity = await _cloudTableHelper.RetrieveEntityAsync<AzureStopEntity>(shortName[0].ToString(), shortName);
            return stopEntity;
        }

        public async Task<List<IStop>> GetAllStopsAsync()
        {
            var stops = await _cloudTableHelper.RetrieveAllEntitiesAsync<AzureStopEntity>();
            return new List<IStop>(stops);
        }
    }
}
