using System;
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

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel = null)
        {
            throw new NotSupportedException("The Application Insight logger is only available as a synchronous logger. Use CreateLogger instead.");
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new ApplicationInsightLogger(null, GetMinimumLogLevel(minimumLogLevel));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            throw new NotSupportedException("The Application Insight logger is only available as a synchronous logger. Use CreateLogger instead.");
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new ApplicationInsightLogger(source, GetMinimumLogLevel(minimumLogLevel));
        }

        public LogLevelEnum GetMinimumLogLevel(LogLevelEnum? minimumLogLevel)
        {
            return minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel);
        }
    }
}
