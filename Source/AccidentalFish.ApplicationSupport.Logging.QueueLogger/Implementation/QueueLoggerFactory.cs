using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Runtime;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation
{
    internal class QueueLoggerFactory : ILoggerFactory
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IApplicationResourceFactory _applicationResourceFactory;
        private readonly IQueueLoggerExtension _queueLoggerExtension;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly LogLevelEnum _defaultMinimumLogLevel;
        private readonly IFullyQualifiedName _defaultLoggerSource;

        public QueueLoggerFactory(
            IRuntimeEnvironment runtimeEnvironment,
            IApplicationResourceFactory applicationResourceFactory,
            IQueueLoggerExtension queueLoggerExtension,
            ICorrelationIdProvider correlationIdProvider,
            LogLevelEnum defaultMinimumLogLevel,
            IFullyQualifiedName defaultLoggerSource)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _applicationResourceFactory = applicationResourceFactory;
            _queueLoggerExtension = queueLoggerExtension;
            _correlationIdProvider = correlationIdProvider;
            _defaultMinimumLogLevel = defaultMinimumLogLevel;
            _defaultLoggerSource = defaultLoggerSource;
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
                _applicationResourceFactory.GetAsynchronousLoggerQueue(),
                source ?? _defaultLoggerSource,
                _queueLoggerExtension,
                minimuLogLevel.GetValueOrDefault(_defaultMinimumLogLevel),
                _correlationIdProvider);
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new QueueLogger(_runtimeEnvironment,
                _applicationResourceFactory.GetLoggerQueue(),
                source ?? _defaultLoggerSource,
                _queueLoggerExtension,
                minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel),
                _correlationIdProvider);
        }
    }
}
