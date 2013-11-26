using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace AccidentalFish.Operations.Diagnostics
{
    public class WorkerRole : RoleEntryPoint
    {
        private IUnityContainer _container;
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ILogger logger = _container.Resolve<ILoggerFactory>().CreateLongLivedLogger(Constants.DiagnosticRole);
            logger.Information("Starting diagnostic role");

            IComponentHost componentHost = _container.Resolve<IComponentHost>();

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
                Trace.TraceInformation("Diagnostic role active");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            _container = new UnityContainer();
            ApplicationSupport.Core.Bootstrapper.RegisterDependencies(_container);
            ApplicationSupport.Azure.Bootstrapper.RegisterDependencies(_container);
            ApplicationSupport.Processes.Bootstrapper.RegistgerDependencies(_container);

            return base.OnStart();
        }
    }
}
