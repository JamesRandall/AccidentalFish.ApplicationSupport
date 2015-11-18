using System;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Email.Amazon
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseAmazonSimpleEmailService(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.Register<IEmailProvider, AmazonSimpleEmailProvider>();
            return dependencyResolver;
        }
    }
}
