using System;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Private;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class LoggerFactory : ILoggerFactory
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IApplicationResourceFactory _applicationResourceFactory;

        public LoggerFactory(IRuntimeEnvironment runtimeEnvironment, IApplicationResourceFactory applicationResourceFactory)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _applicationResourceFactory = applicationResourceFactory;
        }

        public ILogger CreateShortLivedLogger(IFullyQualifiedName source)
        {
            return new Logger(_runtimeEnvironment, _applicationResourceFactory.GetLoggerQueue(), source, GetMinimumLogLevel(source));
        }

        public ILogger CreateShortLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return new Logger(_runtimeEnvironment, queue, source, GetMinimumLogLevel(source));
        }

        public ILogger CreateLongLivedLogger(IFullyQualifiedName source)
        {
            return CreateShortLivedLogger(source);
            // TODO: throw new NotImplementedException("Work in progress - this will be a self refreshing minimum log level component");
        }

        public ILogger CreateLongLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return CreateLongLivedLogger(queue, source, minimumLogLevel);
            // TODO: throw new NotImplementedException("Work in progress - this will be a self refreshing minimum log level component");
        }

        private LogLevelEnum GetMinimumLogLevel(IFullyQualifiedName source)
        {
            // TODO: Pick this up from the realtime settings
            return LogLevelEnum.Debug;
        }
    }
}
