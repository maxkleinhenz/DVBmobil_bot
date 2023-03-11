using Azure;
using Azure.Data.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DVB_Bot.AzureFunctions.Helper
{
    public class TableClientHelper : ITableClientHelper
    {
        private readonly TableClient tableClient;

        public TableClientHelper(TableClient table)
        {
            tableClient = table;
        }

        public async Task<Response> UpsertEntityAsync(ITableEntity entity)
        {
            return await tableClient.UpsertEntityAsync(entity);
        }

        public async Task UpsertEntitiesAsync(
            List<ITableEntity> entities,
            int maxBatchSize = 100,
            CancellationToken cancellationToken = default)
        {
            var addEntitiesBatch = new List<TableTransactionAction>();
            addEntitiesBatch.AddRange(entities.Select(e => new TableTransactionAction(TableTransactionActionType.UpsertMerge, e)));
            var response = await tableClient.SubmitTransactionAsync(addEntitiesBatch).ConfigureAwait(false);
        }

        public async Task<List<T>> QueryAllAsync<T>(CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
        {
            var items = new List<T>();
            var queryResult = tableClient.QueryAsync<T>(filter: "", cancellationToken: cancellationToken);
            await foreach (var r in queryResult)
            {
                items.Add(r);
            }
            return items;
        }

        public async Task<List<T>> QueryAllAsync<T>(string partitionKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
        {
            var items = new List<T>();
            var queryResult = tableClient.QueryAsync<T>(x => x.PartitionKey == partitionKey, cancellationToken: cancellationToken);
            await foreach (var r in queryResult)
            {
                items.Add(r);
            }
            return items;
        }

        public async Task<T> QueryEntityAsync<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
        {
            var entity = await tableClient.GetEntityIfExistsAsync<T>(partitionKey, rowKey, cancellationToken: cancellationToken);
            return entity.HasValue ? entity.Value : null;
        }

        public async Task<Response> DeleteEntityAsync(ITableEntity entity, CancellationToken cancellationToken = default)
        {
            return await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, cancellationToken: cancellationToken);
        }

        public async Task DeleteAllEntitiesAsync<T>(CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            // Only the PartitionKey & RowKey fields are required for deletion
            var entities = tableClient.QueryAsync<TableEntity>(select: new List<string>() { "PartitionKey", "RowKey" }, maxPerPage: 1000, cancellationToken: cancellationToken);

            await entities.AsPages().ForEachAwaitAsync(async page =>
            {
                await BatchManipulateEntities(tableClient, page.Values, TableTransactionActionType.Delete, cancellationToken).ConfigureAwait(false);
            }, cancellationToken);
        }

        /// <summary>
        /// Groups entities by PartitionKey into batches of max 100 for valid transactions
        /// </summary>
        /// <returns>List of Azure Responses for Transactions</returns>
        public static async Task<List<Response<IReadOnlyList<Response>>>> BatchManipulateEntities<T>(TableClient tableClient, IEnumerable<T> entities, TableTransactionActionType tableTransactionActionType, CancellationToken cancellationToken) where T : class, ITableEntity, new()
        {
            var groups = entities.GroupBy(x => x.PartitionKey);
            var responses = new List<Response<IReadOnlyList<Response>>>();
            foreach (var group in groups)
            {
                List<TableTransactionAction> actions;
                var items = group.AsEnumerable();
                while (items.Any())
                {
                    var batch = items.Take(100);
                    items = items.Skip(100);

                    actions = new List<TableTransactionAction>();
                    actions.AddRange(batch.Select(e => new TableTransactionAction(tableTransactionActionType, e)));
                    var response = await tableClient.SubmitTransactionAsync(actions, cancellationToken).ConfigureAwait(false);
                    responses.Add(response);
                }
            }
            return responses;
        }
    }
}
