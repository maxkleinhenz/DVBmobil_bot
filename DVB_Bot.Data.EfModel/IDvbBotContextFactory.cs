namespace DVB_Bot.Data.EfModel
{
    public interface IDvbBotContextFactory
    {
        DvbBotContext CreateContext();
    }
}