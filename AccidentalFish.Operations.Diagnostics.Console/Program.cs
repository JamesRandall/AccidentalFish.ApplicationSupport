using System.Collections.Generic;
using System.Threading;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using Microsoft.Practices.Unity;

namespace AccidentalFish.Operations.Diagnostics.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            ApplicationSupport.Core.Bootstrapper.RegisterDependencies(container);
            ApplicationSupport.Azure.Bootstrapper.RegisterDependencies(container);
            ApplicationSupport.Processes.Bootstrapper.RegisterDependencies(container);

            System.Console.WriteLine("Running infrastructure. Press Ctrl+C to stop.");

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ILogger logger = container.Resolve<ILoggerFactory>().CreateLongLivedLogger(new ComponentIdentity("com.accidentalfish.operations.diagnostics"));
            logger.Information("Starting diagnostic role");

            IComponentHost componentHost = container.Resolve<IComponentHost>();

            componentHost.Start(new StaticComponentHostConfigurationProvider(new List<ComponentConfiguration>
            {
                new ComponentConfiguration
                {
                    ComponentIdentity =
                        new ComponentIdentity(ApplicationSupport.Processes.HostableComponentNames.LogQueueProcessor),
                    Instances = 1,
                    RestartEvaluator = (ex, retryCount) => retryCount < 5
                }
            }), cancellationTokenSource);

            while (true)
            {
                Thread.Sleep(10000);
                System.Console.WriteLine("Diagnostics active. Press Ctrl+C to stop.");
            }
        }
    }
}
