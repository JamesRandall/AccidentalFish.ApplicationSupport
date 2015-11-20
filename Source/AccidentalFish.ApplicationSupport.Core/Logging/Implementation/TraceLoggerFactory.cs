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

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel)
        {
            return new TraceAsynchronousLogger(null, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new TraceLogger(null, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel)
        {
            return new TraceAsynchronousLogger(source, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new TraceLogger(source, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }
    }
}
