using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Email.Amazon
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseAmazonSimpleEmailService(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.Register<IEmailProvider, AmazonSimpleEmailProvider>();
        }
    }
}
