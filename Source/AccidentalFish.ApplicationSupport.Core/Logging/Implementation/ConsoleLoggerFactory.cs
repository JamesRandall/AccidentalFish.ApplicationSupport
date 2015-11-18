using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(LogLevelEnum minimumLogLevel = LogLevelEnum.Warning)
        {
            return new ConsoleLogger(null, minimumLogLevel);
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel = LogLevelEnum.Warning)
        {
            return new ConsoleLogger(source, minimumLogLevel);
        }
    }
}
