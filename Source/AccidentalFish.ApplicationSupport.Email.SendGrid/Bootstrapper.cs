using System;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Email.SendGrid
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseSendGrid(this IDependencyResolver dependencyResolver)
        {
            dependencyResolver.Register<IEmailProvider, SendGridEmailProvider>();
            return dependencyResolver;
        }
    }
}
