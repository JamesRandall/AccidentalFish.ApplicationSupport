using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleAsynchronousLogger(null, minimumLogLevel.GetValueOrDefault(LogLevelEnum.Warning));
        }

        public ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            return new ConsoleLogger(null, minimumLogLevel.GetValueOrDefault(LogLevelEnum.Warning));
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel)
        {
            return new ConsoleAsynchronousLogger(source, minimumLogLevel.GetValueOrDefault(LogLevelEnum.Warning));
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            return new ConsoleLogger(source, minimumLogLevel.GetValueOrDefault(LogLevelEnum.Warning));
        }
    }
}
