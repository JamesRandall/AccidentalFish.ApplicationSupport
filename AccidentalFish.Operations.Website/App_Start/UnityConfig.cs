using System.Web.Mvc;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace AccidentalFish.Operations.Website
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            ApplicationSupport.Core.Bootstrapper.RegisterDependencies(resolver);
            ApplicationSupport.Azure.Bootstrapper.RegisterDependencies(resolver);
            Domain.Bootstrapper.RegisterDependencies(container);
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}