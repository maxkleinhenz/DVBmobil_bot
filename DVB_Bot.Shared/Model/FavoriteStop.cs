using System;
using System.Diagnostics;

namespace DVB_Bot.Shared.Model
{
    [DebuggerDisplay("{ChatId} {StopShortName}")]
    public class FavoriteStop : IFavoriteStop
    {
        public string ChatId { get; set; }
        public string StopShortName { get; set; }
        public DateTime AddDateTime { get; set; }

        public FavoriteStop(string chatId, string stopShortName, DateTime? addDateTime = null)
        {
            ChatId = chatId;
            StopShortName = stopShortName;
            AddDateTime = addDateTime ?? DateTime.Now;
        }
    }
}
