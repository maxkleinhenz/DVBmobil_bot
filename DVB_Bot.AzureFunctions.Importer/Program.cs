using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Shared.Repository;
using DVB_Bot.Telegram.AzureFunctions.Model;
using DVB_Bot.Telegram.Local.Repository;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace DVB_Bot.AzureFunctions.Importer
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var cloudTableClientHelper = new CloudTableClientHelper();
            var tableClient = cloudTableClientHelper.GetTableClient(configuration.GetSection("azure:StorageConnectionString").Value);

            var stopsTable = cloudTableClientHelper.GetCloudTable(tableClient, "Stops");

            var cloudTableHelper = new CloudTableHelper(stopsTable);
            await cloudTableHelper.DeleteAllEntitiesAsync<AzureStopEntity>();

            var stopRepository = new LocalStopRepository();
            await ImportStopsAsync(cloudTableHelper, stopRepository);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static async Task ImportStopsAsync(ICloudTableHelper cloudTableHelper, IStopRepository stopRepository)
        {
            var allStops = (await stopRepository.GetAllStopsAsync()).Select(s => (ITableEntity)new AzureStopEntity(s)).ToList();
            await cloudTableHelper.InsertOrReplaceEntitiesAsync(allStops);
        }
    }
}
