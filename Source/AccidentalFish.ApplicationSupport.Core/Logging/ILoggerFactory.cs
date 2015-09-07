using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    /// <summary>
    /// Creates logger implementations
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Creates a logger with an intended short lifespan for the specified component. Short lived loggers do not auto-refresh their minimum log level once
        /// constructed.
        /// </summary>
        /// <param name="source">The component to log for</param>
        /// <returns>A new logger</returns>
        ILogger CreateShortLivedLogger(IFullyQualifiedName source);

        /// <summary>
        /// Creates a logger with an intended short lifespan for the specified component. Short lived loggers do not auto-refresh their minimum log level once
        /// constructed.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="source">The component to log for</param>
        /// <param name="minimumLogLevel">The minimum log level</param>
        /// <returns>A new logger</returns>
        ILogger CreateShortLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel);

        /// <summary>
        /// Creates a logger with an intended longlifespan for the specified component. Long lived loggers auto-refresh their minimum log level once
        /// constructed however none of the current loggers support auto-refresh and so a short lived logger is constructed.
        /// </summary>
        /// <param name="source">The component to log for</param>
        /// <returns>A new logger</returns>
        ILogger CreateLongLivedLogger(IFullyQualifiedName source);

        /// <summary>
        /// Creates a logger with an intended longlifespan for the specified component. Long lived loggers auto-refresh their minimum log level once
        /// constructed however none of the current loggers support auto-refresh and so a short lived logger is constructed.
        /// </summary>
        /// <param name="queue">The queue to log on</param>
        /// <param name="source">The component to log for</param>
        /// <param name="minimumLogLevel">The minimum log level</param>
        /// <returns>A new logger</returns>
        ILogger CreateLongLivedLogger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLogLevel);
    }
}
