using System.Collections.Generic;
using System.Diagnostics;

namespace DVB_Bot.Shared.Model
{
    [DebuggerDisplay("{StopShortName} {StopName}")]
    public class Departure
    {
        public string StopName { get; set; }
        public string StopShortName { get; set; }
        public List<DepartureRows> DepartureRows { get; set; }
        public DepartureResultState DepartureResultState { get; set; }
        public int RequestedLimit { get; set; }
    }

    public class DepartureRows
    {
        public string Line { get; set; }
        public string FinalStop { get; set; }
        public string ArrivesInMinutes { get; set; }
    }

    public enum DepartureResultState
    {
        Ok,
        StopNotFound,
        Failure,
        DeparturesNotFound
    }
}
