using Azure.Data.Tables;

namespace DVB_Bot.AzureFunctions.Helper
{
    public interface ITableServiceClientHelper
    {
        TableServiceClient CreateTableServiceClient(string storageConnectionString);
        TableClient GetTableClient(TableServiceClient tableServiceClient, string tableName);
    }
}