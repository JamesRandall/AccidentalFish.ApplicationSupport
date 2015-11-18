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
        /// <param name="minimumLogLevel">The minimum log level for the logger</param>
        /// <returns></returns>
        ILogger CreateLogger(LogLevelEnum minimumLogLevel = LogLevelEnum.Warning);

        /// <summary>
        /// Creates a logger
        /// </summary>
        /// <param name="source">The component source for the log items</param>
        /// <param name="minimumLogLevel">The minimum log level for the logger</param>
        /// <returns></returns>
        ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel = LogLevelEnum.Warning);
    }
}
