using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    internal class ApplicationInsightLoggerFactory : ILoggerFactory
    {
        private readonly LogLevelEnum _defaultMinimumLogLevel;

        public ApplicationInsightLoggerFactory(LogLevelEnum defaultMinimumLogLevel)
        {
            _defaultMinimumLogLevel = defaultMinimumLogLevel;
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel)
        {
            return new ApplicationInsightLogger(null, GetMinimumLogLevel(minimumLogLevel));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel)
        {
            return new ApplicationInsightLogger(source, GetMinimumLogLevel(minimumLogLevel));
        }

        public LogLevelEnum GetMinimumLogLevel(LogLevelEnum? minimumLogLevel)
        {
            return minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel);
        }
    }
}
