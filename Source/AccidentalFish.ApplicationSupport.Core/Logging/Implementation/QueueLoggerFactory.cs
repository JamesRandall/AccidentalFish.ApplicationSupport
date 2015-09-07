using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Private;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class QueueLoggerFactory : ILoggerFactory
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IApplicationResourceFactory _applicationResourceFactory;
        private readonly ILoggerExtension _loggerExtension;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public QueueLoggerFactory(IRuntimeEnvironment runtimeEnvironment,
            IApplicationResourceFactory applicationResourceFactory,
            ILoggerExtension loggerExtension,
            ICorrelationIdProvider correlationIdProvider)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _applicationResourceFactory = applicationResourceFactory;
            _loggerExtension = loggerExtension;
            _correlationIdProvider = correlationIdProvider;
        }

        public ILogger CreateShortLivedLogger(IFullyQualifiedName source)
        {
            return new QueueLogger(_runtimeEnvironment, _applicationResourceFactory.GetLoggerQueue(), source, _loggerExtension, GetMinimumLogLevel(source), _correlationIdProvider);
        }

        public ILogger CreateShortLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return new QueueLogger(_runtimeEnvironment, queue, source, _loggerExtension, GetMinimumLogLevel(source), _correlationIdProvider);
        }

        public ILogger CreateLongLivedLogger(IFullyQualifiedName source)
        {
            return CreateShortLivedLogger(source);
            // TODO: throw new NotImplementedException("Work in progress - this will be a self refreshing minimum log level component");
        }

        public ILogger CreateLongLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return CreateShortLivedLogger(queue, source, minimumLogLevel);
            // TODO: throw new NotImplementedException("Work in progress - this will be a self refreshing minimum log level component");
        }

        private LogLevelEnum GetMinimumLogLevel(IFullyQualifiedName source)
        {
            // TODO: Pick this up from the realtime settings
            return LogLevelEnum.Debug;
        }
    }
}
