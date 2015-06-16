using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Injection;

namespace AccidentalFish.ApplicationSupport.Email.Amazon
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.Register<IEmailProvider, AmazonSimpleEmailProvider>();
        }
    }
}
