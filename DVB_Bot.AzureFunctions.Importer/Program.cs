using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Shared.Repository;
using DVB_Bot.Telegram.AzureFunctionsV4.Model;
using DVB_Bot.Telegram.Local.Repository;
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

            var tableServiceClientHelper = new TableServiceClientHelper();
            var tableClient = tableServiceClientHelper.CreateTableServiceClient(configuration.GetSection("azure:StorageConnectionString").Value);

            var stopsTable = tableServiceClientHelper.GetTableClient(tableClient, "Stops");

            var tableClientHelper = new TableClientHelper(stopsTable);
            await tableClientHelper.DeleteAllEntitiesAsync<AzureStopEntity>();

            var stopRepository = new LocalStopRepository();
            await ImportStopsAsync(tableClientHelper, stopRepository);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static async Task ImportStopsAsync(ITableClientHelper ableClientHelper, IStopRepository stopRepository)
        {
            var allStops = (await stopRepository.GetAllStopsAsync()).Select(s => (ITableEntity)new AzureStopEntity(s)).ToList();
            await ableClientHelper.UpsertEntitiesAsync(allStops);
        }
    }
}
