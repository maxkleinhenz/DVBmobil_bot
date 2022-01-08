namespace DVB_Bot.Shared.Model
{
    public interface IStop
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string UrlStop { get; set; }
        public string UrlDeparture { get; set; }
    }
}
