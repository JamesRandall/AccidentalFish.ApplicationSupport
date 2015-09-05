using System;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseAzureApplicationInsightsLogger(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.Register<ILoggerFactory, ApplicationInsightLoggerFactory>();
            return dependencyResolver;
        }

        [Obsolete]
        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            UseAzureApplicationInsightsLogger(dependencyResolver);
        }
    }
}
