using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class TraceLoggerFactory : ILoggerFactory
    {
        private readonly LogLevelEnum _defaultMinimumLogLevel;

        public TraceLoggerFactory(LogLevelEnum defaultMinimumLogLevel)
        {
            _defaultMinimumLogLevel = defaultMinimumLogLevel;
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleLogger(null, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleLogger(source, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }
    }
}
