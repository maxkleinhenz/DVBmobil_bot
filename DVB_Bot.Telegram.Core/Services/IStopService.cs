using DVB_Bot.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVB_Bot.Telegram.Core.Services
{
    public interface IStopService
    {
        Task<Departure> GetDepartureFromShortNameAsync(string shortName, int limit);
        Task<List<IStop>> GetStopsByFuzzySearchAsync(string name);
    }
}