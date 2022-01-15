using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task SendMessageAsync(Chat chat, string message)
        {
            var escapedMessage = EscapeStrings(message);

            await _botClient.SendTextMessageAsync(
                chatId: chat,
                text: escapedMessage,
                parseMode: ParseMode.MarkdownV2,
                disableWebPagePreview: true
            );
        }

        public async Task SendMessageAsync(Chat chat, string message, IReplyMarkup replyMarkup)
        {
            var escapedMessage = EscapeStrings(message);

            await _botClient.SendTextMessageAsync(
                chatId: chat,
                text: escapedMessage,
                parseMode: ParseMode.MarkdownV2,
                replyMarkup: replyMarkup
            );
        }

        private static string EscapeStrings(string value)
        {
            return value.Replace("(", "\\(").Replace(")", "\\)").Replace(".", "\\.").Replace("-", "\\-").Replace("+", "\\+");
        }
    }
}
