using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.Operations.Website.Domain.Services.Implementation
{
    internal class LogViewerService : ILogViewerService
    {
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _byDateDescendingTable;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _bySeverityTable;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _bySourceTable; 

        public LogViewerService(IApplicationResourceFactory applicationResourceFactory)
        {
            IComponentIdentity logQueueProcessorComponentIdentity = new ComponentIdentity("com.accidentalfish.log-queue-processor");
            string byDateDescendingTableName = applicationResourceFactory.Setting(logQueueProcessorComponentIdentity, "logger-bydate-table");
            string bySeverityTableName = applicationResourceFactory.Setting(logQueueProcessorComponentIdentity, "logger-byseverity-table");
            string bySourceTableName = applicationResourceFactory.Setting(logQueueProcessorComponentIdentity, "logger-bysource-table");

            _byDateDescendingTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(byDateDescendingTableName, logQueueProcessorComponentIdentity);
            _bySeverityTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySeverityTableName, logQueueProcessorComponentIdentity);
            _bySourceTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySourceTableName, logQueueProcessorComponentIdentity);
        }

        public async Task<PagedResultSegment<LogTableItem>> GetByDateDescending(string continuationToken)
        {
            return await _byDateDescendingTable.PagedQueryAsync(null, 10, continuationToken);
        }

        public async Task<PagedResultSegment<LogTableItem>> GetBySeverity(string continuationToken)
        {
            return await _bySeverityTable.PagedQueryAsync(null, 10, continuationToken);
        }

        public async Task<PagedResultSegment<LogTableItem>> GetBySource(string continuationToken)
        {
            return await _bySourceTable.PagedQueryAsync(null, 10, continuationToken);
        }
    }
}
