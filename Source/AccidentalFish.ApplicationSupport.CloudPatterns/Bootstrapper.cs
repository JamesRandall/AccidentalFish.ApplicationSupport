using AccidentalFish.ApplicationSupport.CloudPatterns.Implementation;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.CloudPatterns
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseCloudPatterns(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.RegisterInstance<ICircuitBreakerFactory>(new CircuitBreakerFactory());
        }
    }
}
