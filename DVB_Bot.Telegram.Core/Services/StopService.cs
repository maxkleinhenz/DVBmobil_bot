
using DVB_Bot.Shared.Contracts;
using DVB_Bot.Shared.Model;
using DVB_Bot.Telegram.Core.Repository;
using FuzzySharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DVB_Bot.Telegram.Core.Services
{
    public class StopService : IStopService
    {
        public const int DepartureShortLimit = 10;
        public const int DepartureLongLimit = 20;

        public const int FuzzySearchMaxResult = 5;

        private const string StopBaseUrl = "http://widgets.vvo-online.de/abfahrtsmonitor/Haltestelle.do?ort={0}&hst={1}";
        private const string DepartureBaseUrl = "http://widgets.vvo-online.de/abfahrtsmonitor/Abfahrten.do?vz=0&lim={0}&hst={1}";

        private const string StopsDdResource = "DVB_Bot.Core.Assets.stops.csv";

        private IStopRepository _stopRepository;

        public IStopRepository StopRepository
        {
            get => _stopRepository ??= new StopRepository();
            set => _stopRepository = value;
        }

        //public async Task StoreAllStopsInDbFromApiAwait(IProgress<string> progress)
        //{
        //    await StoreStopsAwait(StopsDdResource, progress);
        //}

        public async Task<Departure> GetDepartureFromShortName(string shortName, int limit)
        {
            var stop = StopRepository.GetStopByShortName(shortName);
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

        public List<Stop> GetStopsByFuzzySearch(string name)
        {
            var allStops = StopRepository.GetAllStops();
            var allStopNames = allStops.Select(_ => _.Name).ToArray();
            var extractedResults = Process.ExtractTop(name, allStopNames, limit: FuzzySearchMaxResult);

            var sorted = extractedResults
                .OrderByDescending(_ => _.Score)
                .ThenBy(_ => _.Value)
                .ToList();
            var result = new List<Stop>();
            foreach (var extractedResult in sorted)
            {
                result.Add(allStops[extractedResult.Index]);
            }

            return result;
        }

        //private async Task StoreStopsAwait(string resource, IProgress<string> progress)
        //{
        //    try
        //    {
        //        var errors = new List<string>();

        //        var stops = await ReadResource(resource);

        //        for (int i = 0; i < stops.Length; i++)
        //        {
        //            var stop = stops[i];

        //            var stopCity = stop.Split(';')[0];
        //            var stopName = stop.Split(';')[1];
        //            var stopShortName = stop.Split(';')[2];

        //            progress?.Report($"{i} / {stops.Length} - {stopName}");

        //            var urlStop = string.Format(StopBaseUrl, stopCity, CleanStopName(stopName));
        //            var response = await new HttpClient().GetStringAsync(urlStop);
        //            if (response == "[]")
        //            {
        //                var n = $"{CleanStopName(stopName)}, Dresden";
        //                urlStop = string.Format(StopBaseUrl, stopCity, n);
        //                response = await new HttpClient().GetStringAsync(urlStop);
        //            }

        //            if (response == "[]")
        //            {
        //                errors.Add(urlStop);
        //                continue;
        //            }

        //            var deserialized = JsonConvert.DeserializeObject<string[][][]>(response);
        //            var name = deserialized[1][0][0];
        //            var city = deserialized[1][0][1];
        //            var code = deserialized[1][0][2];

        //            var dbStop = new Stop
        //            {
        //                Name = name,
        //                ShortName = stopShortName,
        //                City = city,
        //                Code = code,
        //                UrlStop = urlStop,
        //                UrlDeparture = DepartureBaseUrl + code
        //            };
        //            await StopRepository.AddStopAsync(dbStop);
        //        }

        //        progress?.Report("Errors:");
        //        foreach (var f in errors)
        //        {
        //            progress?.Report(f);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        //private static string CleanStopName(string stopName)
        //{
        //    return stopName.Replace("ß", "ss");
        //}

        //private static async Task<string[]> ReadResource(string resource)
        //{
        //    var assembly = Assembly.GetExecutingAssembly();

        //    using (var stream = assembly.GetManifestResourceStream(resource))
        //    using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
        //    {
        //        var result = await reader.ReadToEndAsync();
        //        return result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        //    }
        //}
    }
}
