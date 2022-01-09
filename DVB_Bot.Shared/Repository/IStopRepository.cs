using System.Collections.Generic;
using System.Threading.Tasks;
using DVB_Bot.Shared.Model;

namespace DVB_Bot.Shared.Repository
{
    public interface IStopRepository
    {
        Task<IStop> GetStopByShortNameAsync(string shortName);
        Task<List<IStop>> GetAllStopsAsync();
    }
}