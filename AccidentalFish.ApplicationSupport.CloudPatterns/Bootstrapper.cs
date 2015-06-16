using AccidentalFish.ApplicationSupport.CloudPatterns.Implementation;
using AccidentalFish.ApplicationSupport.Injection;

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
