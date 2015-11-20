using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Logging.ConsoleLogger.Implementation
{
    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        private readonly LogLevelEnum _defaultMinimumLoggingLevel;

        public ConsoleLoggerFactory(LogLevelEnum defaultMinimumLoggingLevel)
        {
            _defaultMinimumLoggingLevel = defaultMinimumLoggingLevel;
        }

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleAsynchronousLogger(null, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new ConsoleLogger(null, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleAsynchronousLogger(source, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new ConsoleLogger(source, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }
    }
}
