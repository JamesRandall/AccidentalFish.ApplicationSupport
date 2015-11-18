using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    internal class ApplicationInsightLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(LogLevelEnum minimumLogLevel = LogLevelEnum.Warning)
        {
            return new ApplicationInsightLogger(null, minimumLogLevel);
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel = LogLevelEnum.Warning)
        {
            return new ApplicationInsightLogger(source, minimumLogLevel);
        }
    }
}
