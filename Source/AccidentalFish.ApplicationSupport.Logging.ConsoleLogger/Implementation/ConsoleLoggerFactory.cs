using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Logging.ConsoleLogger.Implementation
{
    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        private readonly LogLevelEnum _defaultMinimumLoggingLevel;
        private readonly IFullyQualifiedName _defaultLoggerSource;

        public ConsoleLoggerFactory(LogLevelEnum defaultMinimumLoggingLevel, IFullyQualifiedName defaultLoggerSource)
        {
            _defaultMinimumLoggingLevel = defaultMinimumLoggingLevel;
            _defaultLoggerSource = defaultLoggerSource;
        }

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleAsynchronousLogger(_defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new ConsoleLogger(_defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleAsynchronousLogger(source ?? _defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new ConsoleLogger(source ?? _defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLoggingLevel));
        }
    }
}
