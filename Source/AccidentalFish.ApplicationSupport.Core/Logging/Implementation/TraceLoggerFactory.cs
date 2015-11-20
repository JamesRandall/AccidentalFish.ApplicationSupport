using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class TraceLoggerFactory : ILoggerFactory
    {
        private readonly LogLevelEnum _defaultMinimumLogLevel;
        private readonly IFullyQualifiedName _defaultLoggerSource;

        public TraceLoggerFactory(LogLevelEnum defaultMinimumLogLevel, IFullyQualifiedName defaultLoggerSource)
        {
            _defaultMinimumLogLevel = defaultMinimumLogLevel;
            _defaultLoggerSource = defaultLoggerSource;
        }

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel)
        {
            return new TraceAsynchronousLogger(null, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new TraceLogger(_defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new TraceAsynchronousLogger(source ?? _defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new TraceLogger(source ?? _defaultLoggerSource, minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel));
        }
    }
}
