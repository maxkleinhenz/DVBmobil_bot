using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DVB_Bot.Telegram.Core
{
    public interface ISendMessageService
    {
        Task SendMessage(Chat chat, string message);
        Task SendMessage(Chat chat, string message, IReplyMarkup replyMarkup);
    }
}