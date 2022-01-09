using DVB_Bot.Shared.Model;
using FuzzySharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DVB_Bot.Shared.Repository;

namespace DVB_Bot.Telegram.Core.Services
{
    public class StopService : IStopService
    {
        public const int DepartureShortLimit = 10;
        public const int DepartureLongLimit = 20;

        public const int FuzzySearchMaxResult = 5;

        private const string StopBaseUrl = "http://widgets.vvo-online.de/abfahrtsmonitor/Haltestelle.do?ort={0}&hst={1}";
        private const string DepartureBaseUrl = "http://widgets.vvo-online.de/abfahrtsmonitor/Abfahrten.do?vz=0&lim={0}&hst={1}";

        private readonly IStopRepository _stopRepository;

        public StopService(IStopRepository stopRepository)
        {
            _stopRepository = stopRepository;
        }

        public async Task<Departure> GetDepartureFromShortNameAsync(string shortName, int limit)
        {
            var stop = await _stopRepository.GetStopByShortNameAsync(shortName);
            if (stop == null)
                return new Departure
                {
                    DepartureResultState = DepartureResultState.StopNotFound
                };

            var urlDeparture = string.Format(DepartureBaseUrl, limit, stop.Code);
            var response = await new HttpClient().GetStringAsync(urlDeparture);
            var deserialized = JsonConvert.DeserializeObject<string[][]>(response);

            var departuresRows = new List<DepartureRows>();
            foreach (var departure in deserialized)
            {
                var departureRow = new DepartureRows
                {
                    Line = departure[0],
                    FinalStop = departure[1],
                    ArrivesInMinutes = departure[2],
                };
                departuresRows.Add(departureRow);
            }

            if (!departuresRows.Any())
            {
                return new Departure
                {
                    DepartureResultState = DepartureResultState.DeparturesNotFound
                };
            }

            return new Departure
            {
                StopName = stop.Name,
                StopShortName = stop.ShortName,
                DepartureRows = departuresRows,
                DepartureResultState = DepartureResultState.Ok,
                RequestedLimit = limit
            };
        }

        public async Task<List<IStop>> GetStopsByFuzzySearchAsync(string name)
        {
            var allStops = await _stopRepository.GetAllStopsAsync();
            var allStopNames = allStops.Select(_ => _.Name).ToArray();
            var extractedResults = Process.ExtractTop(name, allStopNames, limit: FuzzySearchMaxResult);

            var sorted = extractedResults
                .OrderByDescending(_ => _.Score)
                .ThenBy(_ => _.Value)
                .ToList();
            var result = new List<IStop>();
            foreach (var extractedResult in sorted)
            {
                result.Add(allStops[extractedResult.Index]);
            }

            return result;
        }
    }
}
