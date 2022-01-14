using DVB_Bot.Telegram.Core.Commands;
using DVB_Bot.Telegram.Core.Properties;
using DVB_Bot.Telegram.Core.Services;
using System;
using System.Threading.Tasks;
using DVB_Bot.Shared.Repository;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DVB_Bot.Telegram.Core
{
    public class DvbTelegramBot
    {
        public ITelegramBotClient TelegramBotClient { get; }

        private readonly ISendMessageService _sendMessageService;
        private readonly IStopService _stopService;
        private readonly IFavoriteStopService _favoriteStopService;

        public DvbTelegramBot(string telegramBotToken, IStopRepository stopRepository, IFavoriteStopRepository favoriteStopRepository)
        {
            _stopService = new StopService(stopRepository);
            _favoriteStopService = new FavoriteStopService(stopRepository, favoriteStopRepository);

            TelegramBotClient = new TelegramBotClient(telegramBotToken);
            _sendMessageService = new SendMessageService(TelegramBotClient);
        }

        public async Task ComputeMessageAsync(Message telegramMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(telegramMessage.Text))
                    return;

                var messageText = telegramMessage.Text.Trim();
                if (string.IsNullOrEmpty(messageText))
                    return;

                var splittedMessage = messageText.Split(" ");
                var shortName = splittedMessage.Length > 1 ? splittedMessage[1].ToUpper() : null;

                if (string.Equals(splittedMessage[0], Commands.Commands.CommandStart, StringComparison.OrdinalIgnoreCase))
                {
                    // print start message
                    await _sendMessageService.SendMessageAsync(telegramMessage.Chat, Strings.Programm_StartMessage);
                    await _sendMessageService.SendMessageAsync(telegramMessage.Chat, Strings.Programm_HelpMessage);
                }
                else if (string.Equals(splittedMessage[0], Commands.Commands.CommandHelp, StringComparison.OrdinalIgnoreCase))
                {
                    // print help message
                    await _sendMessageService.SendMessageAsync(telegramMessage.Chat, Strings.Programm_HelpMessage);
                }
                else if (string.Equals(splittedMessage[0], Commands.Commands.CommandAddStation, StringComparison.OrdinalIgnoreCase))
                {
                    // add fav
                    if (string.IsNullOrEmpty(shortName))
                    {
                        var m = string.Format(Strings.Program_SpecifyStop, Commands.Commands.CommandAddStation);
                        await _sendMessageService.SendMessageAsync(telegramMessage.Chat, m);
                        return;
                    }

                    var command = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await command.AddFavoriteStopAsync(telegramMessage.Chat, shortName);
                }
                else if (string.Equals(splittedMessage[0], Commands.Commands.CommandRemoveStation, StringComparison.OrdinalIgnoreCase))
                {
                    // remove fav
                    if (string.IsNullOrEmpty(shortName))
                    {
                        var m = string.Format(Strings.Program_SpecifyStop, Commands.Commands.CommandRemoveStation);
                        await _sendMessageService.SendMessageAsync(telegramMessage.Chat, m);
                        return;
                    }

                    var command = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await command.RemoveFavoriteStopAsync(telegramMessage.Chat, shortName);
                }
                else if (string.Equals(splittedMessage[0], Commands.Commands.CommandShowFavoriteStations, StringComparison.OrdinalIgnoreCase))
                {
                    // refresh keys
                    var command = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await command.ShowFavoriteStopsAsync(telegramMessage.Chat);
                }
                else if (splittedMessage[0].StartsWith("/"))
                {
                    // invalid command
                    await _sendMessageService.SendMessageAsync(telegramMessage.Chat, Strings.Program_InvalidCommand);
                    await _sendMessageService.SendMessageAsync(telegramMessage.Chat, Strings.Programm_HelpMessage);
                }
                else
                {
                    // send departures
                    var stopShortName = splittedMessage[0];
                    var command = new ShowDeparturesCommand(_sendMessageService, _stopService, _favoriteStopService);
                    await command.ShowDeparturesAsync(telegramMessage.Chat, stopShortName,
                        StopService.DepartureShortLimit);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public async Task ComputeCallbackQueryAsync(CallbackQuery callbackQuery)
        {
            try
            {
                var query = callbackQuery.Data.Split(Commands.Commands.QueryDataSeparator);
                var command = query[0];
                var shortName = query[1];

                if (string.Equals(command, Commands.Commands.QueryLoadCommand, StringComparison.OrdinalIgnoreCase))
                {
                    var c = new ShowDeparturesCommand(_sendMessageService, _stopService, _favoriteStopService);
                    await c.ShowDeparturesAsync(callbackQuery.Message.Chat, shortName, StopService.DepartureShortLimit);
                }
                else if (string.Equals(command, Commands.Commands.QueryLoadMoreCommand, StringComparison.OrdinalIgnoreCase))
                {
                    var c = new ShowDeparturesCommand(_sendMessageService, _stopService, _favoriteStopService);
                    await c.ShowDeparturesAsync(callbackQuery.Message.Chat, shortName, StopService.DepartureLongLimit);
                }
                else if (string.Equals(command, Commands.Commands.QueryAddToFavoriteCommand, StringComparison.OrdinalIgnoreCase))
                {
                    var f = new FavoriteStopsCommand(_sendMessageService, _favoriteStopService);
                    await f.AddFavoriteStopAsync(callbackQuery.Message.Chat, shortName);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
