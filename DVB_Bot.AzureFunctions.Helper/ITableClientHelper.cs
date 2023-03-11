using Azure;
using Azure.Data.Tables;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DVB_Bot.AzureFunctions.Helper
{
    public interface ITableClientHelper
    {
        Task DeleteAllEntitiesAsync<T>(CancellationToken cancellationToken = default) where T : ITableEntity, new();
        Task<Response> DeleteEntityAsync(ITableEntity entity, CancellationToken cancellationToken = default);
        Task<List<T>> QueryAllAsync<T>(CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task<List<T>> QueryAllAsync<T>(string partitionKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task<T> QueryEntityAsync<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new();
        Task UpsertEntitiesAsync(List<ITableEntity> entities, int maxBatchSize = 100, CancellationToken cancellationToken = default);
        Task<Response> UpsertEntityAsync(ITableEntity entity);
    }
}