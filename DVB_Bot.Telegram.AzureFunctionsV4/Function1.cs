using System.Collections.Generic;
using System.Net;
using DVB_Bot.AzureFunctions.Helper;
using DVB_Bot.Telegram.AzureFunctionsV4.Repository;
using DVB_Bot.Telegram.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DVB_Bot.Telegram.AzureFunctionsV4
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration configuration;

        public Function1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            this.configuration = configuration;
        }

        [Function("getDepartures")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            string requestBody = null;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<TelegramRequestBody>(requestBody);
            

            var tableServiceClientHelper = new TableServiceClientHelper();
            var tableServiceClient = tableServiceClientHelper.CreateTableServiceClient(configuration["StorageConnectionString"]);

            var stopTableClient = tableServiceClientHelper.GetTableClient(tableServiceClient, "Stops");
            var stopRepository = new AzureStopRepository(stopTableClient);

            var favoriteStopTableClient = tableServiceClientHelper.GetTableClient(tableServiceClient, "FavoriteStops");
            var favoriteStopRepository = new AzureFavoriteStopRepository(favoriteStopTableClient);

            var dvbTelegramBot = new DvbTelegramBot(configuration["TelegramBotToken"], stopRepository, favoriteStopRepository);

            if (data?.Message != null)
            {
                await dvbTelegramBot.ComputeMessageAsync(data.Message);
                return OkResponse(req);
            }
            else if (data?.Callback_Query != null)
            {
                await dvbTelegramBot.ComputeCallbackQueryAsync(data.Callback_Query);
                return OkResponse(req);
            }
            else
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString("unexpected request");
                return response;
            }
        }

        private HttpResponseData OkResponse(HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("ok");
            return response;
        }
    }
}
