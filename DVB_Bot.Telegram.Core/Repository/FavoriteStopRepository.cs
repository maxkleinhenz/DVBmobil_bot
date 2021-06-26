using DVB_Bot.Shared.Contracts;
using DVB_Bot.Shared.Model;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVB_Bot.Telegram.Core.Repository
{
    public class FavoriteStopRepository : IFavoriteStopRepository
    {
        private readonly CloudTable _table;
        private readonly StopRepository _stopRepository;

        public FavoriteStopRepository(CloudTable table, StopRepository stopRepository)
        {
            _table = table;
            _stopRepository = stopRepository;
        }

        public async Task<FavoriteStop> AddFavoriteStopAsync(string chatId, Stop stop)
        {
            var favoriteStop = new FavoriteStop(chatId, stop.ShortName);
            var insertOperation = TableOperation.InsertOrReplace(favoriteStop);
            await _table.ExecuteAsync(insertOperation);

            return favoriteStop;
        }

        public async Task<FavoriteStop> RemoveFavoriteStopAsync(string chatId, Stop stop)
        {
            var allFavs = await GetFavoriteStopsAsync(chatId);
            if (allFavs.All(_ => _.ShortName != stop.ShortName))
                return null;

            var favoriteStop = new FavoriteStop(chatId, stop.ShortName) { ETag = "*" };
            var insertOperation = TableOperation.Delete(favoriteStop);
            var result = await _table.ExecuteAsync(insertOperation);

            return favoriteStop;
        }

        public async Task<List<Stop>> GetFavoriteStopsAsync(string chatId)
        {
            var query = new TableQuery
            {
                FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, chatId)
            };

            var continuationToken = new TableContinuationToken();
            var items = await _table.ExecuteQuerySegmentedAsync(query, continuationToken);
            var favoriteStops = items.Select(item => new FavoriteStop(item))
                .OrderBy(_ => _.AddDateTime)
                .ToList();

            var result = new List<Stop>();
            foreach (var favoriteStop in favoriteStops)
            {
                var stop = _stopRepository.GetStopByShortName(favoriteStop.StopShortName);
                if (stop != null)
                    result.Add(stop);
            }

            return result;
        }
    }
}
