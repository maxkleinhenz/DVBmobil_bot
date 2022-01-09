using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVB_Bot.Shared.Repository;

namespace DVB_Bot.Telegram.Core.Services
{
    public class FavoriteStopService : IFavoriteStopService
    {
        public const int MaxFavoriteStops = 12;

        private readonly IStopRepository _stopRepository;
        private readonly IFavoriteStopRepository _favoriteStopRepository;

        public FavoriteStopService(IStopRepository stopRepository, IFavoriteStopRepository favoriteStopRepository)
        {
            _stopRepository = stopRepository;
            _favoriteStopRepository = favoriteStopRepository;
        }

        public async Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId)
        {
            return await _favoriteStopRepository.GetFavoriteStopsAsync(chatId);
        }

        public async Task<bool> IsFavoriteStopAsync(string chatId, string shortName)
        {
            var allFavs = await GetFavoriteStopsAsync(chatId);
            return allFavs.Any(f => string.Equals(f.StopShortName, shortName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<FavoriteStopResult> AddFavoriteStopAsync(string chatId, string shortName)
        {
            var stop = await _stopRepository.GetStopByShortNameAsync(shortName);
            if (stop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopNotFound };

            var existingFavoriteStops = await GetFavoriteStopsAsync(chatId);
            if (existingFavoriteStops.Count == MaxFavoriteStops)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.TooManyFavoriteStops };

            var favoriteStop = await _favoriteStopRepository.AddFavoriteStopAsync(chatId, shortName);
            if (favoriteStop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopAlreadyAdded };

            var allFavoriteStops = await GetFavoriteStopsAsync(chatId);
            return new FavoriteStopResult
            {
                ResultType = FavoriteStopResultTypes.Ok,
                FavoriteStop = favoriteStop,
                AllFavoriteStops = allFavoriteStops
            };
        }

        public async Task<FavoriteStopResult> RemoveFavoriteStopAsync(string chatId, string shortName)
        {
            var favoriteStop = await _favoriteStopRepository.RemoveFavoriteStopAsync(chatId, shortName);
            if (favoriteStop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopIsntFavorite };

            var allFavoriteStops = await GetFavoriteStopsAsync(chatId);
            return new FavoriteStopResult
            {
                ResultType = FavoriteStopResultTypes.Ok,
                FavoriteStop = favoriteStop,
                AllFavoriteStops = allFavoriteStops
            };
        }
    }
}
