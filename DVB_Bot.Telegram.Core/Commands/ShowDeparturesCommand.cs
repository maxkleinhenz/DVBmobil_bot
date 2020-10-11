using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVB_Bot.Shared.Model;
using DVB_Bot.Telegram.Core.Properties;
using DVB_Bot.Telegram.Core.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DVB_Bot.Telegram.Core.Commands
{
    public class ShowDeparturesCommand
    {
        public const int MinLengthLine = 3;
        public const int MinLengthFinalStop = 15;
        public const int MaxLengthFinalStop = 23;
        public const int MinLengthArrivesInMinutes = 2;

        private readonly ISendMessageService _sendMessageService;
        private readonly IStopService _stopService;
        private readonly IFavoriteStopService _favoriteStopService;

        public ShowDeparturesCommand(ISendMessageService sendMessageService, IStopService stopService, IFavoriteStopService favoriteStopService)
        {
            _sendMessageService = sendMessageService;
            _stopService = stopService;
            _favoriteStopService = favoriteStopService;
        }

        public async Task ShowDepartures(Chat chat, string shortName, int limit)
        {
            var departures = await _stopService.GetDepartureFromShortName(shortName, limit);

            if (departures.DepartureResultState == DepartureResultState.Ok)
            {
                await HandleSuccess(chat, shortName, departures);
            }
            else if (departures.DepartureResultState == DepartureResultState.StopNotFound)
            {
                await LoadSuggestions(chat, shortName);
            }
            else
            {
                await HandleError(chat, departures);
            }
        }

        private async Task HandleSuccess(Chat chat, string shortName, Departure departures)
        {
            var message = GetFormattedDepartureMessage(departures);

            var inlineButtons = new List<List<InlineKeyboardButton>>();
            var withLoadMoreCommand = departures.DepartureResultState == DepartureResultState.Ok &&
                                      departures.RequestedLimit == StopService.DepartureShortLimit;
            if (withLoadMoreCommand)
            {
                var button = new InlineKeyboardButton
                {
                    Text = Strings.ShowDeparturesCommand_LoadMoreDepartures,
                    CallbackData = $"{Commands.QueryLoadMoreCommand}{Commands.QueryDataSeparator}{shortName}"
                };
                inlineButtons.Add(new List<InlineKeyboardButton> { button });
            }

            if (!await _favoriteStopService.IsFavoriteStop(chat.Id.ToString(), shortName))
            {
                var button = new InlineKeyboardButton
                {
                    Text = Strings.ShowDeparturesCommand_AddToFavorites,
                    CallbackData = $"{Commands.QueryAddToFavoriteCommand}{Commands.QueryDataSeparator}{shortName}",
                };
                inlineButtons.Add(new List<InlineKeyboardButton> { button });
            }

            var markup = new InlineKeyboardMarkup(inlineButtons);
            await _sendMessageService.SendMessage(chat, message, markup);
        }

        private async Task HandleError(Chat chat, Departure departures)
        {
            var message = departures.DepartureResultState switch
            {
                DepartureResultState.DeparturesNotFound => Strings.ShowDeparturesCommand_CouldNotFoundDepartures,
                DepartureResultState.StopNotFound => Strings.ShowDeparturesCommand_CouldNotFoundStop,
                DepartureResultState.Failure => Strings.ShowDeparturesCommand_Error,
                _ => Strings.ShowDeparturesCommand_Error
            };

            await _sendMessageService.SendMessage(chat, message);
        }
        private async Task LoadSuggestions(Chat chat, string name)
        {
            var stops = _stopService.GetStopsByFuzzySearch(name);
            if (!stops.Any())
            {
                await HandleError(chat, new Departure {DepartureResultState = DepartureResultState.StopNotFound});
                return;
            }

            var message = Strings.ShowDeparturesCommand_DidYouMean;
            var inlineButtons = new List<List<InlineKeyboardButton>>();
            foreach (var stop in stops)
            {
                var button = new InlineKeyboardButton
                {
                    Text = stop.Name,
                    CallbackData = $"{Commands.QueryLoadCommand}{Commands.QueryDataSeparator}{stop.ShortName}"
                };
                inlineButtons.Add(new List<InlineKeyboardButton> {button});
            }

            var markup = new InlineKeyboardMarkup(inlineButtons);
            await _sendMessageService.SendMessage(chat, message, markup);
        }

        private static string GetFormattedDepartureMessage(Departure departures)
        {
            var lengthLine = Math.Max(MinLengthLine, departures.DepartureRows.Max(_ => _.Line.Length));
            var lengthFinalStop = Math.Clamp(departures.DepartureRows.Max(_ => _.FinalStop.Length), MinLengthFinalStop, MaxLengthFinalStop);
            var lengthArrivesInMinutes = Math.Max(MinLengthArrivesInMinutes, departures.DepartureRows.Max(_ => _.ArrivesInMinutes.Length));

            var builder = new StringBuilder();
            builder.AppendLine($"*{departures.StopName}* ({departures.StopShortName})");
            foreach (var row in departures.DepartureRows)
            {
                var finalStop = row.FinalStop;
                if (finalStop.Length > MaxLengthFinalStop)
                    finalStop = finalStop.Substring(0, MaxLengthFinalStop);
                
                var paddedLine = row.Line.PadRight(lengthLine, ' ');
                var paddedFinalStop = finalStop.PadRight(lengthFinalStop, ' ');
                var paddedArrivesInMinutes = row.ArrivesInMinutes.PadRight(lengthArrivesInMinutes, ' ');

                builder.AppendLine($"`{paddedLine} {paddedFinalStop} {paddedArrivesInMinutes}`");
            }

            return builder.ToString();
        }
    }
}

