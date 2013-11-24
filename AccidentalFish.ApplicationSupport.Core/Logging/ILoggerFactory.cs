using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    public interface ILoggerFactory
    {
        /// <summary>
        /// This is the preferred mechanism of creating a logger if the application component and configuration system is being used
        /// </summary>
        ILogger CreateShortLivedLogger(IFullyQualifiedName source);
        
        /// <summary>
        /// This is the preferred mechanism of creating a logger if the application component and configuration system is not in use
        /// </summary>
        ILogger CreateShortLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel);

        ILogger CreateLongLivedLogger(IFullyQualifiedName source);

        ILogger CreateLongLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel);
    }
}
