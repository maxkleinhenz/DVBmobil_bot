using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace DVB_Bot.AzureFunctions.Helper
{
    public interface ICloudTableClientHelper
    {
        CloudTableClient GetTableClient(string storageConnectionString);
        Task<CloudTable> GetCloudTableAsync(CloudTableClient tableClientHelper, string tableName);
    }
}