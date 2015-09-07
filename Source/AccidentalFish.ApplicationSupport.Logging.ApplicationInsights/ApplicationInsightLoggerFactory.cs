using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    internal class ApplicationInsightLoggerFactory : ILoggerFactory
    {
        public ILogger CreateShortLivedLogger(IFullyQualifiedName source)
        {
            return new ApplicationInsightLogger(source, LogLevelEnum.Warning);
        }

        public ILogger CreateShortLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return new ApplicationInsightLogger(source, minimumLogLevel);
        }

        public ILogger CreateLongLivedLogger(IFullyQualifiedName source)
        {
            return new ApplicationInsightLogger(source, LogLevelEnum.Warning);
        }

        public ILogger CreateLongLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            return new ApplicationInsightLogger(source, minimumLogLevel);
        }
    }
}
