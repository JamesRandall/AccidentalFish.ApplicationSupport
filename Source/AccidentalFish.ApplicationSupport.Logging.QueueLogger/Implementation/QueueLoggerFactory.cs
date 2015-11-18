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

        public QueueLoggerFactory(IRuntimeEnvironment runtimeEnvironment,
            IApplicationResourceFactory applicationResourceFactory,
            IQueueLoggerExtension queueLoggerExtension,
            ICorrelationIdProvider correlationIdProvider)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _applicationResourceFactory = applicationResourceFactory;
            _queueLoggerExtension = queueLoggerExtension;
            _correlationIdProvider = correlationIdProvider;
        }

        public ILogger CreateLogger(LogLevelEnum? minimuLogLevel)
        {
            return CreateLogger(new LoggerSource("default"), minimuLogLevel.GetValueOrDefault(LogLevelEnum.Warning));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimuLogLevel)
        {
            return new Logging.QueueLogger.Implementation.QueueLogger(_runtimeEnvironment,
                _applicationResourceFactory.GetLoggerQueue(),
                source,
                _queueLoggerExtension,
                minimuLogLevel.GetValueOrDefault(LogLevelEnum.Warning),
                _correlationIdProvider);
        }
    }
}
