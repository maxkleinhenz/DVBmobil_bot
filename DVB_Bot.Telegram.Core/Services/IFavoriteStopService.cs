using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVB_Bot.Telegram.Core.Services
{
    public interface IFavoriteStopService
    {
        Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId);
        Task<bool> IsFavoriteStopAsync(string chatId, string shortName);
        Task<FavoriteStopResult> AddFavoriteStopAsync(string chatId, string shortName);
        Task<FavoriteStopResult> RemoveFavoriteStopAsync(string chatId, string shortName);
    }
}