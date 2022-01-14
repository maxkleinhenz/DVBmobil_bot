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

        public async Task AddFavoriteStopAsync(Chat chat, string shortName)
        {
            var result = await _favoriteStopService.AddFavoriteStopAsync(chat.Id.ToString(), shortName);

            if (result.IsSuccessful)
            {
                await HandleSuccessAsync(chat, result, Strings.FavoriteStopsCommand_StopHasBeenAddedToFavorites);
            }
            else
            {
                await HandleErrorAsync(chat, result);
            }
        }

        public async Task RemoveFavoriteStopAsync(Chat chat, string shortName)
        {
            var result = await _favoriteStopService.RemoveFavoriteStopAsync(chat.Id.ToString(), shortName);

            if (result.IsSuccessful)
            {
                await HandleSuccessAsync(chat, result, Strings.FavoriteStopsCommand_RemovedFromFavorites);
            }
            else
            {
                await HandleErrorAsync(chat, result);
            }
        }

        public async Task ShowFavoriteStopsAsync(Chat chat)
        {
            var favoriteStops = await _favoriteStopService.GetFavoriteStopsAsync(chat.Id.ToString());
            var keyboardMarkup = CreateReplyKeyboardMarkup(favoriteStops.Select(f => f.StopShortName));
            await _sendMessageService.SendMessageAsync(chat, Strings.FavoriteStopsCommand_RefreshFavorties, keyboardMarkup);
        }

        private static ReplyKeyboardMarkup CreateReplyKeyboardMarkup(IEnumerable<string> shortNames)
        {
            var buttons = shortNames.Select(_ => new KeyboardButton(_)).ToList();

            var nestedButtonList = new List<List<KeyboardButton>>();
            var tempButton = new List<KeyboardButton>();
            for (var i = 0; i < buttons.Count; i++)
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

            return new ReplyKeyboardMarkup(nestedButtonList) { ResizeKeyboard = true };
        }

        private async Task HandleSuccessAsync(Chat chat, FavoriteStopResult result, string message)
        {
            if (!result.AllFavoriteStops.Any())
            {
                await _sendMessageService.SendMessageAsync(chat, message, new ReplyKeyboardRemove());
            }
            else
            {
                var keyboardMarkup = CreateReplyKeyboardMarkup(result.AllFavoriteStops.Select(f => f.StopShortName));
                await _sendMessageService.SendMessageAsync(chat, message, keyboardMarkup);
            }
        }

        private async Task HandleErrorAsync(Chat chat, FavoriteStopResult result)
        {
            switch (result.ResultType)
            {
                case FavoriteStopResultTypes.StopAlreadyAdded:
                    await _sendMessageService.SendMessageAsync(chat, Strings.FavoriteStopsCommand_StopIsAlreadyFavorites);
                    break;
                case FavoriteStopResultTypes.StopIsntFavorite:
                    await _sendMessageService.SendMessageAsync(chat, Strings.FavoriteStopsCommand_StopIsNotFavorite);
                    break;
                case FavoriteStopResultTypes.StopNotFound:
                    await _sendMessageService.SendMessageAsync(chat, Strings.FavoriteStopsCommand_StopCouldNotFound);
                    break;
                case FavoriteStopResultTypes.TooManyFavoriteStops:
                    await _sendMessageService.SendMessageAsync(chat, Strings.FavoriteStopsCommand_MaxFavorites);
                    break;
                case FavoriteStopResultTypes.Ok:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
