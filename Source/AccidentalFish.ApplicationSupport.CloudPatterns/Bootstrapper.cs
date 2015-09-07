using System;
using AccidentalFish.ApplicationSupport.CloudPatterns.Implementation;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.CloudPatterns
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseCloudPatterns(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterInstance<ICircuitBreakerFactory>(new CircuitBreakerFactory());
            return dependencyResolver;
        }

        [Obsolete]
        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            UseCloudPatterns(dependencyResolver);
        }
    }
}
