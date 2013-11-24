using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.InternalMappers;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Private;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.BackgroundProcesses
{
    [ComponentIdentity(HostableComponentNames.LogQueueProcessor)]
    internal class LogQueueProcessor : AbstractApplicationComponent, IHostableComponent
    {
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IMapperFactory _mapperFactory;
        private readonly IAsynchronousQueue<LogQueueItem> _queue;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _bySourceTable;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _bySeverityTable;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _byDateTable;

        public LogQueueProcessor(
            IApplicationResourceFactory applicationResourceFactory,
            IAsynchronousBackoffPolicy backoffPolicy,
            IMapperFactory mapperFactory)
        {
            _queue = applicationResourceFactory.GetLoggerQueue();
            _backoffPolicy = backoffPolicy;
            _mapperFactory = mapperFactory;

            string byDateTableName = applicationResourceFactory.Setting(ComponentIdentity, "logger-bydate-table");
            string bySeverityTableName = applicationResourceFactory.Setting(ComponentIdentity, "logger-byseverity-table");
            string bySourceTableName = applicationResourceFactory.Setting(ComponentIdentity, "logger-bysource-table");

            _bySourceTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySourceTableName, ComponentIdentity);
            _bySeverityTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySeverityTableName, ComponentIdentity);
            _byDateTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(byDateTableName, ComponentIdentity);
        }

        // TODO: This is a bleed through of the component host not being correct
        public async Task Start(CancellationToken cancellationToken)
        {
            _backoffPolicy.Execute(AttemptDequeue, cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500, cancellationToken);
            }
        }

        private async void AttemptDequeue(Action<bool> backoffResultAction)
        {
            try
            {
                await _queue.DequeueAsync(item => ProcessItem(item, backoffResultAction));
            }
            catch (Exception)
            {
                Trace.TraceError("Unable to process queue item");
            }            
        }

        private async Task<bool> ProcessItem(LogQueueItem item, Action<bool> backoffResultAction)
        {
            if (item == null)
            {
                backoffResultAction(false);
                return await Task.FromResult(false);
            }

            IMapper<LogQueueItem, LogTableItem> mapper = _mapperFactory.GetLogQueueItemLogTableItemMapper();

            LogTableItem bySource = mapper.Map(item);
            LogTableItem bySeverity = mapper.Map(item);
            LogTableItem byDate = mapper.Map(item);
            bySource.SetPartitionAndRowKeyForLogBySource();
            bySeverity.SetPartitionAndRowKeyForLogBySeverity();
            byDate.SetPartitionAndRowKeyForLogByDate();

            Task[] tasks = new Task[3];
            bool success = false;
            try
            {
                tasks[0] = _bySourceTable.InsertAsync(bySource);
                tasks[1] = _bySeverityTable.InsertAsync(bySeverity);
                tasks[2] = _byDateTable.InsertAsync(byDate);

                await Task.WhenAll(tasks);
                success = true;
            }
            catch (Exception)
            {
                Trace.TraceError("Unable to store items in log");
            }

            backoffResultAction(true);
            return await Task.FromResult(success);
        }
    }
}
