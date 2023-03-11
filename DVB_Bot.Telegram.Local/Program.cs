using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DVB_Bot.Telegram.Core;
using DVB_Bot.Telegram.Local.Repository;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DVB_Bot.Telegram.Local
{
    internal class Program
    {
        private static DvbTelegramBot _dvbBot;

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var telegramBotToken = configuration.GetSection("telegram:token").Value;

            var stopRepository = new LocalStopRepository();
            var favoriteStopRepository = new LocalFavoriteStopRepository();

            _dvbBot = new DvbTelegramBot(telegramBotToken, stopRepository, favoriteStopRepository);
            
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            _dvbBot.TelegramBotClient.StartReceiving(UpdateHandler, ErrorHandler,
                receiverOptions,
                cts.Token);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            cts.Cancel();
        }

        private static Task<int> ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Debug.WriteLine(exception);
            return Task.FromResult(-1);
        }

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => _dvbBot.ComputeMessageAsync(update.Message),
                UpdateType.CallbackQuery => _dvbBot.ComputeCallbackQueryAsync(update.CallbackQuery),
                _ => throw new ArgumentOutOfRangeException()
            };

            await handler;
        }
    }
}
