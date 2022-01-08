using System;

namespace DVB_Bot.Shared.Model
{
    public interface IFavoriteStop
    {
        public string ChatId { get; set; }
        public string StopShortName { get; set; }
        public DateTime AddDateTime { get; set; }
    }
}
