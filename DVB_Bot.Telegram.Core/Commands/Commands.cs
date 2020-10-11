namespace DVB_Bot.Telegram.Core.Commands
{
    public static class Commands
    {
        public const string CommandStart = "/start";
        public const string CommandHelp = "/help";

        public const string CommandAddStation = "/add";
        public const string CommandRemoveStation = "/remove";
        public const string CommandShowFavoriteStations = "/favs";

        public const string QueryDataSeparator = "_";
        public const string QueryLoadCommand = "load";
        public const string QueryLoadMoreCommand = "loadmore";
        public const string QueryAddToFavoriteCommand = "addtofavs";
    }
}