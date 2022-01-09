using System.Collections.Generic;
using System.Threading.Tasks;
using DVB_Bot.Shared.Model;

namespace DVB_Bot.Shared.Repository
{
    public interface IFavoriteStopRepository
    {
        Task<IFavoriteStop> AddFavoriteStopAsync(string chatId, string stopShortName);
        Task<IFavoriteStop> RemoveFavoriteStopAsync(string chatId, string stopShortName);
        Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId);
    }
}