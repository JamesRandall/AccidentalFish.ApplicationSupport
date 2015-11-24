using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger.Implementation
{
    internal class QueueLoggerFactory : ILoggerFactory
    {
        private static readonly IComponentIdentity ApplicationSupportComponentIdentity = new ComponentIdentity(Constants.ApplicationSupportFqn);
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IQueueSerializer _queueSerializer;
        private readonly IQueueLoggerExtension _queueLoggerExtension;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly LogLevelEnum _defaultMinimumLogLevel;
        private readonly IFullyQualifiedName _defaultLoggerSource;
        private readonly string _queueName;
        private readonly string _storageAccountConnectionString;

        
        public QueueLoggerFactory(
            IRuntimeEnvironment runtimeEnvironment,
            IApplicationResourceSettingNameProvider nameProvider,
            IConfiguration configuration,
            IQueueSerializer queueSerializer,
            IQueueLoggerExtension queueLoggerExtension,
            ICorrelationIdProvider correlationIdProvider,
            LogLevelEnum defaultMinimumLogLevel,
            IFullyQualifiedName defaultLoggerSource)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _queueSerializer = queueSerializer;
            _queueLoggerExtension = queueLoggerExtension;
            _correlationIdProvider = correlationIdProvider;
            _defaultMinimumLogLevel = defaultMinimumLogLevel;
            _defaultLoggerSource = defaultLoggerSource;

            _queueName = configuration[nameProvider.SettingName(ApplicationSupportComponentIdentity, "logger-queue")];
            _storageAccountConnectionString = configuration[nameProvider.StorageAccountConnectionString(ApplicationSupportComponentIdentity)];
        }

        private CloudQueue GetQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageAccountConnectionString);
            CloudQueueClient client = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference(_queueName);
            return queue;
        }

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimuLogLevel)
        {
            return CreateAsynchronousLogger(_defaultLoggerSource, minimuLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return CreateLogger(_defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimuLogLevel)
        {
            return new QueueAsynchronousLogger(_runtimeEnvironment,
                GetQueue(),
                _queueSerializer,
                source ?? _defaultLoggerSource,
                _queueLoggerExtension,
                minimuLogLevel.GetValueOrDefault(_defaultMinimumLogLevel),
                _correlationIdProvider);
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new AzureQueueLogger.Implementation.QueueLogger(_runtimeEnvironment,
                GetQueue(),
                _queueSerializer,
                source ?? _defaultLoggerSource,
                _queueLoggerExtension,
                minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel),
                _correlationIdProvider);
        }
    }
}
