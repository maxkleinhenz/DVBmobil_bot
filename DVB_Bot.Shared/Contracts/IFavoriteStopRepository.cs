using DVB_Bot.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVB_Bot.Shared.Contracts
{
    public interface IFavoriteStopRepository
    {
        Task<FavoriteStop> AddFavoriteStopAsync(string chatId, Stop stop);
        Task<FavoriteStop> RemoveFavoriteStopAsync(string chatId, Stop stop);
        Task<List<Stop>> GetFavoriteStopsAsync(string chatId);
    }
}