using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseAzureApplicationInsightsLogger(this IDependencyResolver dependencyResolver,
            LogLevelEnum defaultMinimumLogLevel = LogLevelEnum.Warning)
        {
            ILoggerFactory loggerFactory = new ApplicationInsightLoggerFactory(defaultMinimumLogLevel);
            dependencyResolver.RegisterInstance(loggerFactory);
            return dependencyResolver;
        }
    }
}
