using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Alerts;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Private;
using AccidentalFish.ApplicationSupport.Processes.Mappers;

namespace AccidentalFish.ApplicationSupport.Processes.Logging
{
    [ComponentIdentity(HostableComponentNames.LogQueueProcessor)]
    internal class LogQueueProcessor : AbstractApplicationComponent, IHostableComponent
    {
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IMapperFactory _mapperFactory;
        private readonly IAlertSender _alertSender;
        private readonly IAsynchronousQueue<LogQueueItem> _queue;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _bySourceTable;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _bySeverityTable;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _byDateDescTable;
        private readonly IAsynchronousNoSqlRepository<LogTableItem> _byDateTable;

        public LogQueueProcessor(
            IApplicationResourceFactory applicationResourceFactory,
            IAsynchronousBackoffPolicy backoffPolicy,
            IMapperFactory mapperFactory,
            IAlertSender alertSender)
        {
            _queue = applicationResourceFactory.GetLoggerQueue();
            _backoffPolicy = backoffPolicy;
            _mapperFactory = mapperFactory;
            _alertSender = alertSender;

            string byDateTableName = applicationResourceFactory.Setting(ComponentIdentity, "logger-bydate-table");
            string byDateDescTableName = applicationResourceFactory.Setting(ComponentIdentity, "logger-bydate-desc-table");
            string bySeverityTableName = applicationResourceFactory.Setting(ComponentIdentity, "logger-byseverity-table");
            string bySourceTableName = applicationResourceFactory.Setting(ComponentIdentity, "logger-bysource-table");

            _bySourceTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySourceTableName, ComponentIdentity);
            _bySeverityTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySeverityTableName, ComponentIdentity);
            _byDateTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(byDateTableName, ComponentIdentity);
            _byDateDescTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(byDateDescTableName, ComponentIdentity);
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

        private async Task<bool> ProcessItem(IQueueItem<LogQueueItem> queueItem, Action<bool> backoffResultAction)
        {
            if (queueItem == null)
            {
                backoffResultAction(false);
                return await Task.FromResult(false);
            }

            LogQueueItem item = queueItem.Item;
            if (item.Level == LogLevelEnum.Error)
            {
                _alertSender.Send(String.Format("SYSTEM ERROR: {0}", item.Source), item.Message);
            }

            IMapper<LogQueueItem, LogTableItem> mapper = _mapperFactory.GetLogQueueItemLogTableItemMapper();

            LogTableItem bySource = mapper.Map(item);
            LogTableItem bySeverity = mapper.Map(item);
            LogTableItem byDateDesc = mapper.Map(item);
            LogTableItem byDate = mapper.Map(item);
            bySource.SetPartitionAndRowKeyForLogBySource();
            bySeverity.SetPartitionAndRowKeyForLogBySeverity();
            byDateDesc.SetPartitionAndRowKeyForLogByDateDesc();
            byDate.SetPartitionAndRowKeyForLogByDate();

            Task[] tasks = new Task[4];
            bool success = false;
            try
            {
                tasks[0] = _bySourceTable.InsertAsync(bySource);
                tasks[1] = _bySeverityTable.InsertAsync(bySeverity);
                tasks[2] = _byDateDescTable.InsertAsync(byDateDesc);
                tasks[3] = _byDateTable.InsertAsync(byDate);

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
