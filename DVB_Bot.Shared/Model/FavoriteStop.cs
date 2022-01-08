using System;

namespace DVB_Bot.Shared.Model
{
    public class FavoriteStop : IFavoriteStop
    {
        public string ChatId { get; set; }
        public string StopShortName { get; set; }
        public DateTime AddDateTime { get; set; }

        public FavoriteStop(string chatId, string stopShortName, DateTime? addDateTime = null)
        {
            this.ChatId = chatId;
            this.StopShortName = stopShortName;
            AddDateTime = addDateTime ?? DateTime.Now;
        }
    }
}
