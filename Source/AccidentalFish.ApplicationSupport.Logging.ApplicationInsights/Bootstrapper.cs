using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.ApplicationInsights.Implementation;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseAzureApplicationInsightsLogger(
            this IDependencyResolver dependencyResolver,
            LogLevelEnum defaultMinimumLogLevel = LogLevelEnum.Warning,
            IFullyQualifiedName defaultLoggerSource = null)
        {
            return dependencyResolver
                .Register<ILoggerFactory>(() => new ApplicationInsightLoggerFactory(defaultMinimumLogLevel, defaultLoggerSource))
                .Register(() => new ApplicationInsightLoggerFactory(defaultMinimumLogLevel, defaultLoggerSource).CreateLogger());
        }
    }
}
