using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using DVB_Bot.Telegram.AzureFunctions.Repository;
using DVB_Bot.Telegram.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace DVB_Bot.Telegram.AzureFunctions
{
    public static class TelegramHttpFunction
    {
        [FunctionName("getUpdates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<TelegramRequestBody>(requestBody);

            var config = CreateConfiguration(context);

            var stopTable = await GetCloudTable(config, "Stops");
            var stopRepository = new AzureStopRepository(stopTable);

            var favoriteStopTable = await GetCloudTable(config, "FavoriteStops");
            var favoriteStopRepository = new AzureFavoriteStopRepository(favoriteStopTable);

            var dvbTelegramBot = new DvbTelegramBot(config["TelegramBotToken"], stopRepository, favoriteStopRepository);

            if (data.Message != null)
            {
                await dvbTelegramBot.ComputeMessage(data.Message);
            }
            else if (data.Callback_Query != null)
            {
                await dvbTelegramBot.ComputeCallbackQuery(data.Callback_Query);
            }
            else
            {
                return new BadRequestErrorMessageResult("unexpected request");
            }

            return new OkObjectResult("ok");
        }

        private static IConfiguration CreateConfiguration(ExecutionContext context)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            var config = configBuilder.Build();
            return config;
        }

        private static async Task<CloudTable> GetCloudTable(IConfiguration config, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(config["StorageConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();
            var favoriteStopTable = tableClient.GetTableReference(tableName);
            await favoriteStopTable.CreateIfNotExistsAsync();

            return favoriteStopTable;
        }
    }
}
