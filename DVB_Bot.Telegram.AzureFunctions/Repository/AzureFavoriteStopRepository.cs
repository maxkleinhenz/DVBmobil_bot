using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVB_Bot.Shared.Contracts;
using DVB_Bot.Shared.Model;
using DVB_Bot.Telegram.AzureFunctions.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Telegram.AzureFunctions.Repository
{
    public class AzureFavoriteStopRepository : IFavoriteStopRepository
    {
        private readonly CloudTable _table;

        public AzureFavoriteStopRepository(CloudTable table)
        {
            _table = table;
        }

        public async Task<IFavoriteStop> AddFavoriteStopAsync(string chatId, string stopShortName)
        {
            var favoriteStop = new AzureFavoriteStop(chatId, stopShortName);
            var insertOperation = TableOperation.InsertOrReplace(favoriteStop);
            await _table.ExecuteAsync(insertOperation);

            return favoriteStop;
        }

        public async Task<IFavoriteStop> RemoveFavoriteStopAsync(string chatId, string stopShortName)
        {
            var allFavs = await GetFavoriteStopsAsync(chatId);
            if (allFavs.All(f => f.StopShortName != stopShortName))
                return null;

            var favoriteStop = new AzureFavoriteStop(chatId, stopShortName) { ETag = "*" };
            var insertOperation = TableOperation.Delete(favoriteStop);
            var result = await _table.ExecuteAsync(insertOperation);

            return favoriteStop;
        }

        public async Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId)
        {
            var query = new TableQuery
            {
                FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, chatId)
            };

            var continuationToken = new TableContinuationToken();
            var items = await _table.ExecuteQuerySegmentedAsync(query, continuationToken);
            var favoriteStops = items.Select(item => (IFavoriteStop)new AzureFavoriteStop(item))
                .OrderBy(_ => _.AddDateTime)
                .ToList();
            return favoriteStops;
        }
    }
}
