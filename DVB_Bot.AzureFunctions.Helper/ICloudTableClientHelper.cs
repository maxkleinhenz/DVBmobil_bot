using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.AzureFunctions.Helper
{
    public interface ICloudTableClientHelper
    {
        CloudTableClient GetTableClient(string storageConnectionString);
        Task<CloudTable> GetCloudTableAsync(CloudTableClient tableClientHelper, string tableName);
    }
}