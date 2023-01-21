using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;


namespace DVB_Bot.AzureFunctions.Helper
{
    public class CloudTableClientHelper : ICloudTableClientHelper
    {
        public CloudTableClient GetTableClient(string storageConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            return storageAccount.CreateCloudTableClient();
        }

        public CloudTable GetCloudTable(CloudTableClient tableClientHelper, string tableName)
        {
            var favoriteStopTable = tableClientHelper.GetTableReference(tableName);
            return favoriteStopTable;
        }

        public async Task<CloudTable> GetCloudTableAndCreateAsync(CloudTableClient tableClientHelper, string tableName)
        {
            var favoriteStopTable = tableClientHelper.GetTableReference(tableName);
            await favoriteStopTable.CreateIfNotExistsAsync();
            return favoriteStopTable;
        }
    }
}
