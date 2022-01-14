using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace DVB_Bot.AzureFunctions.Helper
{
    public interface ICloudTableHelper
    {
        Task<TableResult> InsertOrReplaceEntityAsync(ITableEntity entity);

        Task InsertOrReplaceEntitiesAsync(
            List<ITableEntity> entities,
            int maxBatchSize = 100,
            CancellationToken cancellationToken = default);

        Task<List<T>> RetrieveAllEntitiesAsync<T>(CancellationToken cancellationToken = default) where T : ITableEntity, new();
        Task<List<T>> RetrieveEntitiesByPartitionKeyAsync<T>(string partitionKey) where T : class, ITableEntity, new();
        Task<T> RetrieveEntityAsync<T>(string partitionKey, string rowKey) where T : ITableEntity, new();
        Task<TableResult> DeleteEntityAsync(ITableEntity entity, string etag = "*");
        Task DeleteAllEntitiesAsync<T>(int maxBatchSize = 100, CancellationToken cancellationToken = default) where T : ITableEntity, new();
    }
}