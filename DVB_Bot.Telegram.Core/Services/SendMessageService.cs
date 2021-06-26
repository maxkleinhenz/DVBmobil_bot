﻿using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DVB_Bot.Telegram.Core.Services
{
    public class SendMessageService : ISendMessageService
    {
        private readonly ITelegramBotClient _botClient;

        public SendMessageService(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendMessage(Chat chat, string message)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chat,
                text: message,
                parseMode: ParseMode.Markdown
            );
        }

        public async Task SendMessage(Chat chat, string message, IReplyMarkup replyMarkup)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chat,
                text: message,
                parseMode: ParseMode.Markdown,
                replyMarkup: replyMarkup
            );
        }
    }
}
