using Azure.Data.Tables;

namespace DVB_Bot.AzureFunctions.Helper
{
    public class TableServiceClientHelper : ITableServiceClientHelper
    {
        public TableServiceClient CreateTableServiceClient(string storageConnectionString)
        {
            var tableServiceClient = new TableServiceClient(storageConnectionString);
            return tableServiceClient;
        }

        public TableClient GetTableClient(TableServiceClient tableClientHelper, string tableName)
        {
            var tableClient = tableClientHelper.GetTableClient(tableName);
            return tableClient;
        }
    }
}
