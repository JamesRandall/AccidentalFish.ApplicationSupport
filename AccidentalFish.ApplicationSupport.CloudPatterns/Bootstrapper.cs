using AccidentalFish.ApplicationSupport.CloudPatterns.Implementation;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.CloudPatterns
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterInstance<ICircuitBreakerFactory>(new CircuitBreakerFactory());
        }
    }
}
