using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DVB_Bot.Telegram.Core;
using DVB_Bot.Telegram.Local.Repository;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DVB_Bot.Telegram.Local
{
    internal class Program
    {
        private static DvbTelegramBot dvbBot;

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var telegramBotToken = configuration.GetSection("telegram:token").Value;

            var stopRepository = new LocalStopRepository();
            var favoriteStopRepository = new LocalFavoriteStopRepository();

            dvbBot = new DvbTelegramBot(telegramBotToken, stopRepository, favoriteStopRepository);
            
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            dvbBot.TelegramBotClient.StartReceiving(UpdateHandler, ErrorHandler,
                receiverOptions,
                cts.Token);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            cts.Cancel();
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception arg2, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => dvbBot.ComputeMessageAsync(update.Message),
                UpdateType.CallbackQuery => dvbBot.ComputeCallbackQueryAsync(update.CallbackQuery),
                _ => throw new ArgumentOutOfRangeException()
            };

            await handler;
        }
    }
}
