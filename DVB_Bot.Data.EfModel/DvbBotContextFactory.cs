using System;
using System.Collections.Generic;
using System.Text;

namespace DVB_Bot.Data.EfModel
{
    public class DvbBotContextFactory : IDvbBotContextFactory
    {
        private readonly string _connectionString;

        public DvbBotContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DvbBotContext CreateContext()
        {
            return new DvbBotContext(_connectionString);
        }
    }
}
