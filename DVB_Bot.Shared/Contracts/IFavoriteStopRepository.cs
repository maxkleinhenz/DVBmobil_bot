using DVB_Bot.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVB_Bot.Shared.Contracts
{
    public interface IFavoriteStopRepository
    {
        Task<IFavoriteStop> AddFavoriteStopAsync(string chatId, string stopShortName);
        Task<IFavoriteStop> RemoveFavoriteStopAsync(string chatId, string stopShortName);
        Task<List<IFavoriteStop>> GetFavoriteStopsAsync(string chatId);
    }
}