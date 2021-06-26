using DVB_Bot.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVB_Bot.Telegram.Core.Services
{
    public interface IStopService
    {
        Task<Departure> GetDepartureFromShortName(string shortName, int limit);
        List<Stop> GetStopsByFuzzySearch(string name);
    }
}