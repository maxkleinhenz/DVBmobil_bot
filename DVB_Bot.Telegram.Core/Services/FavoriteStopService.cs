using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVB_Bot.Shared.Contracts;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Results;
using DVB_Bot.Telegram.Core.Repository;

namespace DVB_Bot.Telegram.Core.Services
{
    public class FavoriteStopService : IFavoriteStopService
    {
        public const int MaxFavoriteStops = 12;

        private readonly IFavoriteStopRepository _favoriteStopRepository;

        private IStopRepository _stopRepository;

        public IStopRepository StopRepository
        {
            get => _stopRepository ??= new StopRepository();
            set => _stopRepository = value;
        }

        public FavoriteStopService(IFavoriteStopRepository favoriteStopRepository)
        {
            _favoriteStopRepository = favoriteStopRepository;
        }

        public async Task<List<Stop>> GetFavoriteStops(string chatId)
        {
            return await _favoriteStopRepository.GetFavoriteStopsAsync(chatId);
        }

        public async Task<bool> IsFavoriteStop(string chatId, string shortName)
        {
            var allFavs = await GetFavoriteStops(chatId);
            return allFavs.Any(_ => string.Equals(_.ShortName, shortName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<FavoriteStopResult> AddFavoriteStop(string chatId, string shortName)
        {
            var stop = StopRepository.GetStopByShortName(shortName);
            if (stop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopNotFound };

            var existingFavoriteStops = await GetFavoriteStops(chatId);
            if (existingFavoriteStops.Count == MaxFavoriteStops)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.TooManyFavoriteStops };

            var favoriteStop = await _favoriteStopRepository.AddFavoriteStopAsync(chatId, stop);
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
            var stop = StopRepository.GetStopByShortName(shortName);
            if (stop == null)
                return new FavoriteStopResult { ResultType = FavoriteStopResultTypes.StopNotFound };

            var favoriteStop = await _favoriteStopRepository.RemoveFavoriteStopAsync(chatId, stop);
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
