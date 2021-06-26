using DVB_Bot.Telegram.Core.Commands;
using DVB_Bot.Telegram.Core.Properties;
using DVB_Bot.Telegram.Core.Repository;
using DVB_Bot.Telegram.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DVB_Bot.Telegram.Core
{
    public class DvbTelegramBot
    {
        private readonly ISendMessageService _sendMessageService;
        private readonly IStopService _stopService;
        private readonly IFavoriteStopService _favoriteStopService;

        public DvbTelegramBot(IConfiguration config, CloudTable favoriteStopTable)
        {
            _stopService = new StopService();
            _favoriteStopService =
                new FavoriteStopService(new FavoriteStopRepository(favoriteStopTable, new StopRepository()));

            var token = config["TelegramBotToken"];
            var botClient = new TelegramBotClient(token);
            _sendMessageService = new SendMessageService(botClient);
        }

        //private static void Main(string[] args)
        //{
        //    BotClient = new TelegramBotClient(Config["TelegramBotToken"]);
        //    SendMessageService = new SendMessageService(BotClient);

        //    BotClient.OnMessage += BotClientOnOnMessage;
        //    BotClient.OnCallbackQuery += BotClientOnOnCallbackQuery;
        //    BotClient.StartReceiving();

        //    Console.WriteLine("Press any key to exit");
        //    Console.ReadKey();

        //    BotClient.StopReceiving();

        //    //await InitDvbBotDBAsync();
        //}

        public async Task ComputeMessage(Message messsage)
        {
            try
            {
                if (string.IsNullOrEmpty(messsage.Text))
                    return;

                var message = messsage.Text.Trim();
                if (string.IsNullOrEmpty(message))
                    return;

                var splittedMessage = message.Split(" ");
                var shortName = splittedMessage.Length > 1 ? splittedMessage[1] : null;

                if (splittedMessage[0] == Commands.Commands.CommandStart)
                {
                    // print start message
                    await _sendMessageService.SendMessage(messsage.Chat, Strings.Programm_StartMessage);
                    await _sendMessageService.SendMessage(messsage.Chat, Strings.Programm_HelpMessage);
                }
                else if (splittedMessage[0] == Commands.Commands.CommandHelp)
                {
                    // print help message
                    await _sendMessageService.SendMessage(messsage.Chat, Strings.Programm_HelpMessage);
                }
                else if (splittedMessage[0] == Commands.Commands.CommandAddStation)
                {
                    // add fav
                    if (string.IsNullOrEmpty(shortName))
                    {
                        var m = string.Format(Strings.Program_SpecifyStop, Commands.Commands.CommandAddStation);
                        await _sendMessageService.SendMessage(messsage.Chat, m);
                        return;
                    }

                    var command = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await command.AddFavoriteStop(messsage.Chat, shortName);
                }
                else if (splittedMessage[0] == Commands.Commands.CommandRemoveStation)
                {
                    // remove fav
                    if (string.IsNullOrEmpty(shortName))
                    {
                        var m = string.Format(Strings.Program_SpecifyStop, Commands.Commands.CommandRemoveStation);
                        await _sendMessageService.SendMessage(messsage.Chat, m);
                        return;
                    }

                    var command = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await command.RemoveFavoriteStop(messsage.Chat, shortName);
                }
                else if (splittedMessage[0] == Commands.Commands.CommandShowFavoriteStations)
                {
                    // refresh keys
                    var command = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await command.ShowFavoriteStops(messsage.Chat);
                }
                else if (splittedMessage[0].StartsWith("/"))
                {
                    // invalid command
                    await _sendMessageService.SendMessage(messsage.Chat, Strings.Program_InvalidCommand);
                    await _sendMessageService.SendMessage(messsage.Chat, Strings.Programm_HelpMessage);
                }
                else
                {
                    // send departures
                    var stopShortName = splittedMessage[0];
                    var command = new ShowDeparturesCommand(_sendMessageService, _stopService, _favoriteStopService);
                    await command.ShowDepartures(messsage.Chat, stopShortName,
                        StopService.DepartureShortLimit);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public async Task ComputeCallbackQuery(CallbackQuery callbackQuery)
        {
            try
            {
                var query = callbackQuery.Data.Split(Commands.Commands.QueryDataSeparator);
                var command = query[0];
                var shortName = query[1];

                if (command == Commands.Commands.QueryLoadCommand)
                {
                    var c = new ShowDeparturesCommand(_sendMessageService, _stopService, _favoriteStopService);
                    await c.ShowDepartures(callbackQuery.Message.Chat, shortName, StopService.DepartureShortLimit);
                }
                else if (command == Commands.Commands.QueryLoadMoreCommand)
                {
                    var c = new ShowDeparturesCommand(_sendMessageService, _stopService, _favoriteStopService);
                    await c.ShowDepartures(callbackQuery.Message.Chat, shortName, StopService.DepartureLongLimit);
                }
                else if (command == Commands.Commands.QueryAddToFavoriteCommand)
                {
                    var f = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await f.AddFavoriteStop(callbackQuery.Message.Chat, shortName);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //private async Task InitDvbBotDBAsync()
        //{
        //    var progress = new Progress<string>(Console.WriteLine);
        //    await _stopService.StoreAllStopsInDbFromApiAwait(progress);
        //}
    }
}
