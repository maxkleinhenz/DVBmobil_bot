using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Telegram.AzureFunctions.Repository;
using DVB_Bot.Telegram.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
            string requestBody = null;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<TelegramRequestBody>(requestBody);

            var config = CreateConfiguration(context);

            var cloudTableClientHelper = new CloudTableClientHelper();
            var tableClient = cloudTableClientHelper.GetTableClient(config["StorageConnectionString"]);

            var stopTable = cloudTableClientHelper.GetCloudTable(tableClient, "Stops");
            var stopRepository = new AzureStopRepository(stopTable);

            var favoriteStopTable = cloudTableClientHelper.GetCloudTable(tableClient, "FavoriteStops");
            var favoriteStopRepository = new AzureFavoriteStopRepository(favoriteStopTable);

            var dvbTelegramBot = new DvbTelegramBot(config["TelegramBotToken"], stopRepository, favoriteStopRepository);

            if (data?.Message != null)
            {
                await dvbTelegramBot.ComputeMessageAsync(data.Message);
            }
            else if (data?.Callback_Query != null)
            {
                await dvbTelegramBot.ComputeCallbackQueryAsync(data.Callback_Query);
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
    }
}
