using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Components;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.Operations.Website.Domain.Model;

namespace AccidentalFish.Operations.Website.Domain.Services.Implementation
{
    internal class LogViewerService : ILogViewerService
    {
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _byDateDescendingTable;
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _bySeverityTable;
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _bySourceTable; 

        public LogViewerService(IAzureApplicationResourceFactory applicationResourceFactory)
        {
            IComponentIdentity logQueueProcessorComponentIdentity = new ComponentIdentity("com.accidentalfish.log-queue-processor");
            string byDateDescendingTableName = applicationResourceFactory.Setting(logQueueProcessorComponentIdentity, "logger-bydate-table");
            string bySeverityTableName = applicationResourceFactory.Setting(logQueueProcessorComponentIdentity, "logger-byseverity-table");
            string bySourceTableName = applicationResourceFactory.Setting(logQueueProcessorComponentIdentity, "logger-bysource-table");

            _byDateDescendingTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(byDateDescendingTableName, logQueueProcessorComponentIdentity);
            _bySeverityTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(bySeverityTableName, logQueueProcessorComponentIdentity);
            _bySourceTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(bySourceTableName, logQueueProcessorComponentIdentity);
        }

        public async Task<PagedResultSegment<LogTableItem>> GetByDateDescending(string continuationToken)
        {
            return await _byDateDescendingTable.PagedQueryAsync((Dictionary<string, object>)null, 10, continuationToken);
        }

        public async Task<PagedResultSegment<LogTableItem>> GetBySeverity(string continuationToken)
        {
            return await _bySeverityTable.PagedQueryAsync((Dictionary<string, object>)null, 10, continuationToken);
        }

        public async Task<PagedResultSegment<LogTableItem>> GetBySource(string continuationToken)
        {
            return await _bySourceTable.PagedQueryAsync((Dictionary<string, object>)null, 10, continuationToken);
        }
    }
}
