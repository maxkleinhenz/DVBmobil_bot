using DVB_Bot.Shared.Model;
using System.Collections.Generic;

namespace DVB_Bot.Shared.Contracts
{
    public interface IStopRepository
    {
        Stop GetStopByShortName(string shortName);
        List<Stop> GetAllStops();
    }
}