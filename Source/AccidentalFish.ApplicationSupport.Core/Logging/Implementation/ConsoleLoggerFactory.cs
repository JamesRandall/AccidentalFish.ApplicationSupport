using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleLogger(null, minimumLogLevel.GetValueOrDefault(LogLevelEnum.Warning));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleLogger(source, minimumLogLevel.GetValueOrDefault(LogLevelEnum.Warning));
        }
    }
}
