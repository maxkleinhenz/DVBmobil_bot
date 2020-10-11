using System.Collections.Generic;
using System.Threading.Tasks;
using DVB_Bot.Shared.Model;
using DVB_Bot.Shared.Results;

namespace DVB_Bot.Telegram.Core.Services
{
    public interface IFavoriteStopService
    {
        Task<List<Stop>> GetFavoriteStops(string chatId);
        Task<bool> IsFavoriteStop(string chatId, string shortName);
        Task<FavoriteStopResult> AddFavoriteStop(string chatId, string shortName);
        Task<FavoriteStopResult> RemoveFavoriteStop(string chatId, string shortName);
    }
}