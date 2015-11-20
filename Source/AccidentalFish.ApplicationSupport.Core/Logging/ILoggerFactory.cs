using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    /// <summary>
    /// Creates logger implementations
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Creates a logger
        /// </summary>
        /// <param name="minimumLogLevel">The minimum log level for the logger. If null then the default log level for the implementation will be used.</param>
        /// <returns>A configured logger</returns>
        IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel = null);

        /// <summary>
        /// Creates a logger
        /// </summary>
        /// <param name="minimumLogLevel">The minimum log level for the logger. If null then the default log level for the implementation will be used.</param>
        /// <returns>A configured logger</returns>
        ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null);

        /// <summary>
        /// Creates a logger
        /// </summary>
        /// <param name="source">The component source for the log items</param>
        /// <param name="minimumLogLevel">The minimum log level for the logger. If null then the default log level for the implementation will be used.</param>
        /// <returns>A configured logger</returns>
        IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null);

        /// <summary>
        /// Creates a logger
        /// </summary>
        /// <param name="source">The component source for the log items</param>
        /// <param name="minimumLogLevel">The minimum log level for the logger. If null then the default log level for the implementation will be used.</param>
        /// <returns>A configured logger</returns>
        ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null);
    }
}
