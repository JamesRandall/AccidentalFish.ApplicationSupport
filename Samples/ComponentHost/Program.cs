using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace ComponentHost
{

    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);
            Bootstrapper.RegisterDependencies(resolver, null, Bootstrapper.LoggerTypeEnum.Console, "correlation-id");
            resolver.Register<IHostableComponent, ExampleHostableComponent>(ExampleHostableComponent.FullyQualifiedName);

            IComponentHost componentHost = resolver.Resolve<IComponentHost>();
            componentHost.Start(new StaticComponentHostConfigurationProvider(new List<ComponentConfiguration>
            {
                new ComponentConfiguration
                {
                    ComponentIdentity = new ComponentIdentity(ExampleHostableComponent.FullyQualifiedName),
                    Instances = 1,
                    RestartEvaluator = (ex, retryCount) =>
                    {
                        RestartHandler(ex, retryCount,
                            new ComponentIdentity(ExampleHostableComponent.FullyQualifiedName));
                        return true;
                    }
                }
            }), new CancellationTokenSource());

            Console.Read();
        }

        private static void RestartHandler(Exception ex, int retryCount, IComponentIdentity componentIdentity)
        {
            if (retryCount%5 == 0)
            {
                Console.WriteLine("Pausing for 5 seconds");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }

    internal class ExampleHostableComponent : IHostableComponent
    {
        public const string FullyQualifiedName = "example.component";
        public IComponentIdentity ComponentIdentity
        {
            get { return  new ComponentIdentity(FullyQualifiedName);}
        }

        public Task Start(CancellationToken token)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            throw new Exception("Simulating an error");
        }
    }
}
