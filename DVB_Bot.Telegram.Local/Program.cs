using System;
using System.IO;
using DVB_Bot.Telegram.Core;
using DVB_Bot.Telegram.Local.Repository;
using Microsoft.Extensions.Configuration;

namespace DVB_Bot.Telegram.Local
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var telegramBotToken = configuration.GetSection("telegram:token").Value;

            var stopRepository = new LocalStopRepository();
            var favoriteStopRepository = new LocalFavoriteStopRepository();

            var dvbBot = new DvbTelegramBot(telegramBotToken, stopRepository, favoriteStopRepository);
            dvbBot.TelegramBotClient.OnMessage += async (sender, eventArgs) =>
            {
                await dvbBot.ComputeMessageAsync(eventArgs.Message);
            };

            dvbBot.TelegramBotClient.OnCallbackQuery += async (sender, eventArgs) =>
            {
                await dvbBot.ComputeCallbackQueryAsync(eventArgs.CallbackQuery);
            };

            dvbBot.TelegramBotClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            dvbBot.TelegramBotClient.StopReceiving();
        }
    }
}
