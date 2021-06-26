using DVB_Bot.Shared.Results;
using DVB_Bot.Telegram.Core.Properties;
using DVB_Bot.Telegram.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DVB_Bot.Telegram.Core.Commands
{
    public class FavoriteStopsCommand
    {
        private const int ButtonPerRow = 4;

        private readonly ISendMessageService _sendMessageService;
        private readonly IFavoriteStopService _favoriteStopService;

        public FavoriteStopsCommand(ISendMessageService sendMessageService, IFavoriteStopService favoriteStopService)
        {
            _sendMessageService = sendMessageService;
            _favoriteStopService = favoriteStopService;
        }

        public async Task AddFavoriteStop(Chat chat, string shortName)
        {
            var result = await _favoriteStopService.AddFavoriteStop(chat.Id.ToString(), shortName);

            if (result.IsSuccessful)
            {
                await HandleSuccess(chat, result, Strings.FavoriteStopsCommand_StopHasBeenAddedToFavorites);
            }
            else
            {
                await HandleError(chat, result);
            }
        }

        public async Task RemoveFavoriteStop(Chat chat, string shortName)
        {
            var result = await _favoriteStopService.RemoveFavoriteStop(chat.Id.ToString(), shortName);

            if (result.IsSuccessful)
            {
                await HandleSuccess(chat, result, Strings.FavoriteStopsCommand_RemovedFromFavorites);
            }
            else
            {
                await HandleError(chat, result);
            }
        }

        public async Task ShowFavoriteStops(Chat chat)
        {
            var stops = await _favoriteStopService.GetFavoriteStops(chat.Id.ToString());
            var keyboardMarkup = CreateReplyKeyboardMarkup(stops.Select(_ => _.ShortName));
            await _sendMessageService.SendMessage(chat, Strings.FavoriteStopsCommand_RefreshFavorties, keyboardMarkup);
        }

        private ReplyKeyboardMarkup CreateReplyKeyboardMarkup(IEnumerable<string> shortNames)
        {
            var buttons = shortNames.Select(_ => new KeyboardButton(_)).ToList();

            var nestedButtonList = new List<List<KeyboardButton>>();
            var tempButton = new List<KeyboardButton>();
            for (int i = 0; i < buttons.Count; i++)
            {
                tempButton.Add(buttons[i]);
                if (i % ButtonPerRow == ButtonPerRow - 1)
                {
                    nestedButtonList.Add(tempButton);
                    tempButton = new List<KeyboardButton>();
                }
            }
            if (tempButton.Any())
                nestedButtonList.Add(tempButton);

            return new ReplyKeyboardMarkup(nestedButtonList, resizeKeyboard: true);
        }

        private async Task HandleSuccess(Chat chat, FavoriteStopResult result, string message)
        {
            if (!result.AllFavoriteStops.Any())
            {
                await _sendMessageService.SendMessage(chat, message, new ReplyKeyboardRemove());
            }
            else
            {
                var keyboardMarkup = CreateReplyKeyboardMarkup(result.AllFavoriteStops.Select(_ => _.ShortName));
                await _sendMessageService.SendMessage(chat, message, keyboardMarkup);
            }
        }

        private async Task HandleError(Chat chat, FavoriteStopResult result)
        {
            switch (result.ResultType)
            {
                case FavoriteStopResultTypes.StopAlreadyAdded:
                    await _sendMessageService.SendMessage(chat, Strings.FavoriteStopsCommand_StopIsAlreadyFavorites);
                    break;
                case FavoriteStopResultTypes.StopIsntFavorite:
                    await _sendMessageService.SendMessage(chat, Strings.FavoriteStopsCommand_StopIsNotFavorite);
                    break;
                case FavoriteStopResultTypes.StopNotFound:
                    await _sendMessageService.SendMessage(chat, Strings.FavoriteStopsCommand_StopCouldNotFound);
                    break;
                case FavoriteStopResultTypes.TooManyFavoriteStops:
                    await _sendMessageService.SendMessage(chat, Strings.FavoriteStopsCommand_MaxFavorites);
                    break;
                case FavoriteStopResultTypes.Ok:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
