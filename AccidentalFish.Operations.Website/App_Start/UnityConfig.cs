using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace AccidentalFish.Operations.Website
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            ApplicationSupport.Core.Bootstrapper.RegisterDependencies(container);
            ApplicationSupport.Azure.Bootstrapper.RegisterDependencies(container);
            Domain.Bootstrapper.RegisterDependencies(container);
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}