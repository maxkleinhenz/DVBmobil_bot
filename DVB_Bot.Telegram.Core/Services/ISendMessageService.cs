using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DVB_Bot.Telegram.Core.Services
{
    public interface ISendMessageService
    {
        Task SendMessageAsync(Chat chat, string message);
        Task SendMessageAsync(Chat chat, string message, IReplyMarkup replyMarkup);
    }
}