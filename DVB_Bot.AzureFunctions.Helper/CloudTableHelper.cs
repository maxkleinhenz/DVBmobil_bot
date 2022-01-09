using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.AzureFunctions.Helper
{
    public class CloudTableHelper : ICloudTableHelper
    {
        private readonly CloudTable _table;

        public CloudTableHelper(CloudTable table)
        {
            _table = table;
        }

        public async Task<TableResult> InsertOrReplaceEntityAsync(ITableEntity entity)
        {
            var insertOperation = TableOperation.InsertOrReplace(entity);
            return await _table.ExecuteAsync(insertOperation);
        }

        public async Task InsertOrReplaceEntitiesAsync(
            List<ITableEntity> entities,
            int maxBatchSize = 100,
            CancellationToken cancellationToken = default)
        {
            // max batch size is 100
            if (maxBatchSize <= 0 || maxBatchSize > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(maxBatchSize),
                    $"{nameof(maxBatchSize)} must be between 1 and 100");

            // All entities in a batch must have the same PartitionKey
            var partitionKeys = entities.Select(e => e.PartitionKey).Distinct().OrderBy(e => e);
            foreach (var partitionKey in partitionKeys)
            {
                if (cancellationToken.IsCancellationRequested) break;

                var entitiesSamePartition = entities.Where(e => e.PartitionKey == partitionKey).ToList();

                for (var batchIndex = 0; batchIndex < entitiesSamePartition.Count; batchIndex += maxBatchSize)
                {
                    var batchOperation = new TableBatchOperation();

                    var entityMinIndex = batchIndex * maxBatchSize;
                    var entityMaxIndex = Math.Min(entityMinIndex + maxBatchSize, entitiesSamePartition.Count);

                    for (var i = entityMinIndex; i < entityMaxIndex; i++)
                    {
                        var entity = entitiesSamePartition[i];
                        batchOperation.Add(TableOperation.InsertOrReplace(entity));
                    }

                    if (cancellationToken.IsCancellationRequested) break;

                    _ = await _table.ExecuteBatchAsync(batchOperation);
                    batchOperation.Clear();
                }
            }
        }

        public async Task<List<T>> RetrieveAllEntitiesAsync<T>(CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            var items = new List<T>();
            TableContinuationToken token = null;
            do
            {
                var seg = await _table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token);
                token = seg.ContinuationToken;
                items.AddRange(seg);

            } while (token != null && !cancellationToken.IsCancellationRequested);

            return items.ToList();
        }

        public async Task<List<T>> RetrieveEntitiesByPartitionKeyAsync<T>(string partitionKey) where T : class, ITableEntity, new()
        {
            var query = new TableQuery<T>
            {
                FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey)
            };

            var continuationToken = new TableContinuationToken();
            var items = await _table.ExecuteQuerySegmentedAsync<T>(query, continuationToken);
            return items.ToList();
        }

        public async Task<T> RetrieveEntityAsync<T>(string partitionKey, string rowKey) where T : ITableEntity, new()
        {
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            return (T)(await _table.ExecuteAsync(operation)).Result;
        }

        public async Task<TableResult> DeleteEntityAsync(ITableEntity entity, string etag = "*")
        {
            entity.ETag = etag;
            var insertOperation = TableOperation.Delete(entity);
            return await _table.ExecuteAsync(insertOperation);
        }

        public async Task DeleteAllEntitiesAsync<T>(int maxBatchSize = 100, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            if (maxBatchSize <= 0 || maxBatchSize > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(maxBatchSize),
                    $"{nameof(maxBatchSize)} must be between 1 and 100");


            // query all rows and create batch delete operations
            TableContinuationToken token = null;
            var pendingOperations = new Dictionary<string, Stack<TableOperation>>();
            do
            {
                var result = await _table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token);
                foreach (var row in result)
                {
                    var op = TableOperation.Delete(row);
                    if (pendingOperations.ContainsKey(row.PartitionKey))
                    {
                        pendingOperations[row.PartitionKey].Push(op);
                    }
                    else
                    {
                        pendingOperations.Add(row.PartitionKey, new Stack<TableOperation>());
                        pendingOperations[row.PartitionKey].Push(op);
                    }
                }

                token = result.ContinuationToken;
            } while (token != null && !cancellationToken.IsCancellationRequested);

            //execute batch operations
            var batchOperation = new TableBatchOperation();
            foreach (var key in pendingOperations.Keys)
            {
                var rowStack = pendingOperations[key];
                var current = 0;

                while (rowStack.Count != 0 && !cancellationToken.IsCancellationRequested)
                {
                    // dequeue in groups of 100
                    while (current < maxBatchSize && rowStack.Count > 0 && !cancellationToken.IsCancellationRequested)
                    {
                        var op = rowStack.Pop();
                        batchOperation.Add(op);
                        current++;
                    }

                    //execute and reset
                    _ = await _table.ExecuteBatchAsync(batchOperation);
                    current = 0;
                    batchOperation.Clear();
                }
            }
        }
    }
}
