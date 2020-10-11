using System.Resources;

namespace DVB_Bot.Telegram.Core.Services
{
    public class TextService
    {
        private readonly ResourceManager _resourceManager;

        public TextService(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }


    }
}
