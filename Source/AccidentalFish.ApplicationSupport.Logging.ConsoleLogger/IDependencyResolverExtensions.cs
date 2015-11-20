using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.ConsoleLogger.Implementation;

namespace AccidentalFish.ApplicationSupport.Logging.ConsoleLogger
{
    // ReSharper disable once InconsistentNaming
    public static class IDependencyResolverExtensions
    {
        public static IDependencyResolver UseConsoleLogger(this IDependencyResolver dependencyResolver,
            LogLevelEnum defaultMinimumLogLevel = LogLevelEnum.Warning,
            IFullyQualifiedName defaultLoggerSource = null)
        {
            return dependencyResolver.Register<ILoggerFactory>(() => new ConsoleLoggerFactory(defaultMinimumLogLevel, defaultLoggerSource));
        }
    }
}
