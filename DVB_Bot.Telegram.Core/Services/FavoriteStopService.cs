using DVB_Bot.Shared.Contracts;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<IFavoriteStop>> GetFavoriteStops(string chatId)
        {
            return await _favoriteStopRepository.GetFavoriteStopsAsync(chatId);
        }

        public async Task<bool> IsFavoriteStop(string chatId, string shortName)
        {
            var allFavs = await GetFavoriteStops(chatId);
            return allFavs.Any(f => string.Equals(f.StopShortName, shortName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<FavoriteStopResult> AddFavoriteStop(string chatId, string shortName)
        {
            var stop = await _stopRepository.GetStopByShortNameAsync(shortName);
            if (stop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopNotFound };

            var existingFavoriteStops = await GetFavoriteStops(chatId);
            if (existingFavoriteStops.Count == MaxFavoriteStops)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.TooManyFavoriteStops };

            var favoriteStop = await _favoriteStopRepository.AddFavoriteStopAsync(chatId, shortName);
            if (favoriteStop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopAlreadyAdded };

            var allFavoriteStops = await GetFavoriteStops(chatId);
            return new FavoriteStopResult
            {
                ResultType = FavoriteStopResultTypes.Ok,
                FavoriteStop = favoriteStop,
                AllFavoriteStops = allFavoriteStops
            };
        }

        public async Task<FavoriteStopResult> RemoveFavoriteStop(string chatId, string shortName)
        {
            var favoriteStop = await _favoriteStopRepository.RemoveFavoriteStopAsync(chatId, shortName);
            if (favoriteStop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopIsntFavorite };

            var allFavoriteStops = await GetFavoriteStops(chatId);
            return new FavoriteStopResult
            {
                ResultType = FavoriteStopResultTypes.Ok,
                FavoriteStop = favoriteStop,
                AllFavoriteStops = allFavoriteStops
            };
        }
    }
}
