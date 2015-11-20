using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Email.SendGrid
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseSendGrid(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.Register<IEmailProvider, SendGridEmailProvider>();
        }
    }
}
