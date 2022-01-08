using DVB_Bot.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVB_Bot.Shared.Contracts
{
    public interface IStopRepository
    {
        Task<IStop> GetStopByShortNameAsync(string shortName);
        Task<List<IStop>> GetAllStopsAsync();
    }
}