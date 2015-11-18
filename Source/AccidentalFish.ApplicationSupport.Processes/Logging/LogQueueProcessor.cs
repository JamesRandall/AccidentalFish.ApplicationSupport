using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Components;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Alerts;
using AccidentalFish.ApplicationSupport.Core.BackoffProcesses;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Mappers;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;
using AccidentalFish.ApplicationSupport.Processes.Mappers;

namespace AccidentalFish.ApplicationSupport.Processes.Logging
{
    [ComponentIdentity(HostableComponentNames.LogQueueProcessor)]
    internal class LogQueueProcessor : BackoffQueueProcessor<LogQueueItem>
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IAlertSender _alertSender;
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _bySourceTable;
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _bySeverityTable;
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _byDateDescTable;
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _byDateTable;
        private readonly IAsynchronousTableStorageRepository<LogTableItem> _byCorrelationIdTable; 

        public LogQueueProcessor(
            IAzureApplicationResourceFactory applicationResourceFactory,
            IAsynchronousBackoffPolicy backoffPolicy,
            IMapperFactory mapperFactory,
            IAlertSender alertSender) : base(backoffPolicy, applicationResourceFactory.GetLoggerQueue())
        {
            _mapperFactory = mapperFactory;
            _alertSender = alertSender;

            IComponentIdentity componentIdentity = new ComponentIdentity(HostableComponentNames.LogQueueProcessor);

            string byDateTableName = applicationResourceFactory.Setting(componentIdentity, "logger-bydate-table");
            string byDateDescTableName = applicationResourceFactory.Setting(componentIdentity, "logger-bydate-desc-table");
            string bySeverityTableName = applicationResourceFactory.Setting(componentIdentity, "logger-byseverity-table");
            string bySourceTableName = applicationResourceFactory.Setting(componentIdentity, "logger-bysource-table");
            string byCorrelationIdTableName = applicationResourceFactory.Setting(componentIdentity, "logger-bycorrelationid-table");

            if (!string.IsNullOrWhiteSpace(bySourceTableName))
            {
                _bySourceTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(bySourceTableName, componentIdentity);
            }

            if (!string.IsNullOrWhiteSpace(bySeverityTableName))
            {
                _bySeverityTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(bySeverityTableName, componentIdentity);
            }

            if (!string.IsNullOrWhiteSpace(byDateTableName))
            {
                _byDateTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(byDateTableName, componentIdentity);
            }

            if (!string.IsNullOrWhiteSpace(byDateDescTableName))
            {
                _byDateDescTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(byDateDescTableName, componentIdentity);
            }

            if (!string.IsNullOrWhiteSpace(byCorrelationIdTableName))
            {
                _byCorrelationIdTable = applicationResourceFactory.GetTableStorageRepository<LogTableItem>(byCorrelationIdTableName, componentIdentity);
            }
        }

        protected override async Task<bool> HandleRecievedItemAsync(IQueueItem<LogQueueItem> queueItem)
        {
            LogQueueItem item = queueItem.Item;
            if (item.Level == LogLevelEnum.Error)
            {
                await _alertSender.SendAsync($"SYSTEM ERROR: {item.Source}", item.Message);
            }

            IMapper<LogQueueItem, LogTableItem> mapper = _mapperFactory.GetLogQueueItemLogTableItemMapper();

            List<Task> tasks = new List<Task>();
            if (_bySourceTable != null)
            {
                LogTableItem bySource = mapper.Map(item);
                bySource.SetPartitionAndRowKeyForLogBySource();
                tasks.Add(_bySourceTable.InsertAsync(bySource));
            }

            if (_bySeverityTable != null)
            {
                LogTableItem bySeverity = mapper.Map(item);
                bySeverity.SetPartitionAndRowKeyForLogBySeverity();
                tasks.Add(_bySeverityTable.InsertAsync(bySeverity));
            }

            if (_byDateDescTable != null)
            {
                LogTableItem byDateDesc = mapper.Map(item);
                byDateDesc.SetPartitionAndRowKeyForLogByDateDesc();
                tasks.Add(_byDateDescTable.InsertAsync(byDateDesc));
            }

            if (_byDateTable != null)
            {
                LogTableItem byDate = mapper.Map(item);
                byDate.SetPartitionAndRowKeyForLogByDate();
                tasks.Add(_byDateTable.InsertAsync(byDate));
            }

            if (_byCorrelationIdTable != null && !string.IsNullOrWhiteSpace(item.CorrelationId))
            {
                LogTableItem byCorrelationId = mapper.Map(item);
                byCorrelationId.SetPartitionAndRowKeyForLogByCorrelationId();
                tasks.Add(_byCorrelationIdTable.InsertAsync(byCorrelationId));
            }
            
            await Task.WhenAll(tasks);
            return true;
        }

        public override IComponentIdentity ComponentIdentity => new ComponentIdentity(HostableComponentNames.LogQueueProcessor);
    }
}
