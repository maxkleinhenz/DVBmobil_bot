using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace DVB_Bot.AzureFunctions.Helper
{
    public interface ICloudTableClientHelper
    {
        CloudTableClient GetTableClient(string storageConnectionString);
        CloudTable GetCloudTable(CloudTableClient tableClientHelper, string tableName);
        Task<CloudTable> GetCloudTableAndCreateAsync(CloudTableClient tableClientHelper, string tableName);
    }
}