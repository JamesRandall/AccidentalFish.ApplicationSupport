using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights.Implementation
{
    internal class ApplicationInsightLoggerFactory : ILoggerFactory
    {
        private readonly LogLevelEnum _defaultMinimumLogLevel;
        private readonly IFullyQualifiedName _defaultLoggerSource;

        public ApplicationInsightLoggerFactory(
            LogLevelEnum defaultMinimumLogLevel,
            IFullyQualifiedName defaultLoggerSource)
        {
            _defaultMinimumLogLevel = defaultMinimumLogLevel;
            _defaultLoggerSource = defaultLoggerSource;
        }

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new ApplicationInsightAsynchronousLogger(_defaultLoggerSource, GetMinimumLogLevel(minimumLogLevel));
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new ApplicationInsightLogger(_defaultLoggerSource, GetMinimumLogLevel(minimumLogLevel));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new ApplicationInsightAsynchronousLogger(source ?? _defaultLoggerSource, GetMinimumLogLevel(minimumLogLevel));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new ApplicationInsightLogger(source ?? _defaultLoggerSource, GetMinimumLogLevel(minimumLogLevel));
        }

        public LogLevelEnum GetMinimumLogLevel(LogLevelEnum? minimumLogLevel)
        {
            return minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel);
        }
    }
}
