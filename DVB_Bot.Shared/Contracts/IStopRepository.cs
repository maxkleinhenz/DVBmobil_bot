using System.Collections.Generic;
using DVB_Bot.Shared.Model;

namespace DVB_Bot.Shared.Contracts
{
    public interface IStopRepository
    {
        Stop GetStopByShortName(string shortName);
        List<Stop> GetAllStops();

    }
}