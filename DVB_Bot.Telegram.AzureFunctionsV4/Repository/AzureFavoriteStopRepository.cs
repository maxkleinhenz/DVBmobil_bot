using Azure.Data.Tables;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Repository;
using DVB_Bot.Telegram.AzureFunctionsV4.Model;

namespace DVB_Bot.Telegram.AzureFunctionsV4.Repository
{
    public class AzureFavoriteStopRepository : IFavoriteStopRepository
    {
        private readonly ITableClientHelper tableClientHelper;

        public AzureFavoriteStopRepository(TableClient tableClient)
        {
            tableClientHelper = new TableClientHelper(tableClient);
        }

        public async Task<IFavoriteStop> AddFavoriteStopAsync(string chatId, string stopShortName)
        {
            var favoriteStop = new AzureFavoriteStop(chatId, stopShortName);
            await tableClientHelper.UpsertEntityAsync(favoriteStop);

            return favoriteStop;
        }

        public async Task<IFavoriteStop?> RemoveFavoriteStopAsync(string chatId, string stopShortName)
        {
            var allFavoriteStops = await GetFavoriteStopsAsync(chatId);
            if (allFavoriteStops.All(f => !string.Equals(f.StopShortName, stopShortName, StringComparison.OrdinalIgnoreCase)))
                return null;

            var favoriteStop = new AzureFavoriteStop(chatId, stopShortName);
            await tableClientHelper.DeleteEntityAsync(favoriteStop);

            return favoriteStop;
        }

        public async Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId)
        {
            var favoriteStops = await tableClientHelper.QueryAllAsync<AzureFavoriteStop>(chatId);
            return new List<IFavoriteStop>(favoriteStops.OrderBy(_ => _.AddDateTime).ToList());
        }
    }
}
