using DVB_Bot.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVB_Bot.Telegram.Core.Services
{
    public interface IStopService
    {
        Task<Departure> GetDepartureFromShortName(string shortName, int limit);
        Task<List<IStop>> GetStopsByFuzzySearch(string name);
    }
}