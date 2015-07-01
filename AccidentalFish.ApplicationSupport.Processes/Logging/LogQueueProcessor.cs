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
using AccidentalFish.ApplicationSupport.Core.BackoffProcesses;

namespace AccidentalFish.ApplicationSupport.Processes.Logging
{
    [ComponentIdentity(HostableComponentNames.LogQueueProcessor)]
    internal class LogQueueProcessor : BackoffQueueProcessor<LogQueueItem>
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
            : base(backoffPolicy, applicationResourceFactory.GetLoggerQueue())
        {
            _queue = applicationResourceFactory.GetLoggerQueue();
            _backoffPolicy = backoffPolicy;
            _mapperFactory = mapperFactory;
            _alertSender = alertSender;

            IComponentIdentity componentIdentity = new ComponentIdentity(HostableComponentNames.LogQueueProcessor);

            string byDateTableName = applicationResourceFactory.Setting(componentIdentity, "logger-bydate-table");
            string byDateDescTableName = applicationResourceFactory.Setting(componentIdentity, "logger-bydate-desc-table");
            string bySeverityTableName = applicationResourceFactory.Setting(componentIdentity, "logger-byseverity-table");
            string bySourceTableName = applicationResourceFactory.Setting(componentIdentity, "logger-bysource-table");

            _bySourceTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySourceTableName, componentIdentity);
            _bySeverityTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(bySeverityTableName, componentIdentity);
            _byDateTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(byDateTableName, componentIdentity);
            _byDateDescTable = applicationResourceFactory.GetNoSqlRepository<LogTableItem>(byDateDescTableName, componentIdentity);
        }

        protected override async Task<bool> HandleRecievedItem(IQueueItem<LogQueueItem> queueItem)
        {
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

            return success;
        }

        public override IComponentIdentity ComponentIdentity
        {
            get { return new ComponentIdentity(HostableComponentNames.LogQueueProcessor); }
        }
    }
}
