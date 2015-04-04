using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Email;

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
