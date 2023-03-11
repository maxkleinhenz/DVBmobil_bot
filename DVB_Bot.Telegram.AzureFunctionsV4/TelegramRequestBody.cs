﻿using Telegram.Bot.Types;

namespace DVB_Bot.Telegram.AzureFunctionsV4
{
    public class TelegramRequestBody
    {
        public int Update_Id { get; set; }
        public Message Message { get; set; }
        public CallbackQuery Callback_Query { get; set; }
    }
}
