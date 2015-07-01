using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger CreateShortLivedLogger(IFullyQualifiedName source)
        {
            return new ConsoleLogger();
        }

        public ILogger CreateShortLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return new ConsoleLogger();
        }

        public ILogger CreateLongLivedLogger(IFullyQualifiedName source)
        {
            return new ConsoleLogger();
        }

        public ILogger CreateLongLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return new ConsoleLogger();
        }
    }
}
