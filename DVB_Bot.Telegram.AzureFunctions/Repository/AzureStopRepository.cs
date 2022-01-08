using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DVB_Bot.Shared.Contracts;
using DVB_Bot.Shared.Model;
using DVB_Bot.Telegram.AzureFunctions.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace DVB_Bot.Telegram.AzureFunctions.Repository
{
    public class AzureStopRepository : IStopRepository
    {
        private readonly CloudTable _table;

        public AzureStopRepository(CloudTable table)
        {
            _table = table;
        }

        public async Task<IStop> GetStopByShortNameAsync(string shortName)
        {
            var retrieveJeffSmith = TableOperation.Retrieve<AzureStopEntity>(shortName[0].ToString(), shortName);
            var stopEntity = (AzureStopEntity)(await _table.ExecuteAsync(retrieveJeffSmith)).Result;

            return stopEntity;
        }

        public async Task<List<IStop>> GetAllStopsAsync()
        {
            var items = new List<IStop>();
            var ct = default(CancellationToken);
            TableContinuationToken token = null;
            do
            {
                var seg = await _table.ExecuteQuerySegmentedAsync(new TableQuery<AzureStopEntity>(), token);
                token = seg.ContinuationToken;
                items.AddRange(seg);

            } while (token != null && !ct.IsCancellationRequested);

            return items.ToList();
        }
    }
}
