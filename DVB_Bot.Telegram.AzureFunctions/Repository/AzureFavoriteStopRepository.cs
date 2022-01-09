using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Repository;
using DVB_Bot.Telegram.AzureFunctions.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Telegram.AzureFunctions.Repository
{
    public class AzureFavoriteStopRepository : IFavoriteStopRepository
    {
        private readonly CloudTableHelper _cloudTableHelper;

        public AzureFavoriteStopRepository(CloudTable table)
        {
            _cloudTableHelper = new CloudTableHelper(table);
        }

        public async Task<IFavoriteStop> AddFavoriteStopAsync(string chatId, string stopShortName)
        {
            var favoriteStop = new AzureFavoriteStop(chatId, stopShortName);
            await _cloudTableHelper.InsertOrReplaceEntityAsync(favoriteStop);

            return favoriteStop;
        }

        public async Task<IFavoriteStop> RemoveFavoriteStopAsync(string chatId, string stopShortName)
        {
            var allFavs = await GetFavoriteStopsAsync(chatId);
            if (allFavs.All(f => f.StopShortName != stopShortName))
                return null;

            var favoriteStop = new AzureFavoriteStop(chatId, stopShortName);
            await _cloudTableHelper.DeleteEntityAsync(favoriteStop);

            return favoriteStop;
        }

        public async Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId)
        {
            var favoriteStops = await _cloudTableHelper.RetrieveEntitiesByPartitionKeyAsync<AzureFavoriteStop>(chatId);
            return new List<IFavoriteStop>(favoriteStops.OrderBy(_ => _.AddDateTime).ToList());
        }
    }
}
