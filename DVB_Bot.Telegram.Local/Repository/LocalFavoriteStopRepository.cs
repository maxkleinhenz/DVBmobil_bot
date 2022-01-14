using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Repository;

namespace DVB_Bot.Telegram.Local.Repository
{
    public class LocalFavoriteStopRepository : IFavoriteStopRepository
    {
        private readonly List<IFavoriteStop> _favStops = new List<IFavoriteStop>();

        public Task<IFavoriteStop> AddFavoriteStopAsync(string chatId, string stopShortName)
        {
            var favStop = new FavoriteStop(chatId, stopShortName);
            if (!_favStops.Any(f => f.ChatId == chatId && string.Equals(f.StopShortName, stopShortName, StringComparison.OrdinalIgnoreCase)))
            {
                _favStops.Add(favStop);
            }

            return Task.FromResult((IFavoriteStop)favStop);
        }

        public Task<IFavoriteStop> RemoveFavoriteStopAsync(string chatId, string stopShortName)
        {
            var favStop = _favStops.SingleOrDefault(f => f.ChatId == chatId && string.Equals(f.StopShortName, stopShortName, StringComparison.OrdinalIgnoreCase));

            if (favStop == null)
                return Task.FromResult((IFavoriteStop)new FavoriteStop(chatId, stopShortName));

            _favStops.Remove(favStop);
            return Task.FromResult(favStop);
        }

        public Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId)
        {
            return Task.FromResult(_favStops.Where(f => f.ChatId == chatId).ToList());
        }
    }
}
