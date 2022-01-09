using System.Diagnostics;

namespace DVB_Bot.Shared.Model
{
    [DebuggerDisplay("{ShortName} {Name}")]
    public class Stop : IStop
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string UrlStop { get; set; }
        public string UrlDeparture { get; set; }

        public Stop(string shortName, string name, string city, string code, string urlStop, string urlDeparture)
        {
            ShortName = shortName;
            Name = name;
            City = city;
            Code = code;
            UrlStop = urlStop;
            UrlDeparture = urlDeparture;
        }
    }
}
