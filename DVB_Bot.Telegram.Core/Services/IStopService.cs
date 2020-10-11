using System.Collections.Generic;
using System.Threading.Tasks;
using DVB_Bot.Shared.Model;

namespace DVB_Bot.Telegram.Core.Services
{
    public interface IStopService
    {
        //Task StoreAllStopsInDbFromApiAwait(IProgress<string> progress);
        Task<Departure> GetDepartureFromShortName(string shortName, int limit);
        List<Stop> GetStopsByFuzzySearch(string name);
    }
}