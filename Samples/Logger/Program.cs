using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);

            AccidentalFish.ApplicationSupport.Core.Bootstrapper.RegisterDependencies(dependencyResolver);
            AccidentalFish.ApplicationSupport.Azure.Bootstrapper.RegisterDependencies(dependencyResolver);
            AccidentalFish.ApplicationSupport.Processes.Bootstrapper.RegisterDependencies(dependencyResolver);

            IComponentHost componentHost = dependencyResolver.Resolve<IComponentHost>();
            ILoggerFactory loggerFactory = dependencyResolver.Resolve<ILoggerFactory>();
            ILogger logger = loggerFactory.CreateShortLivedLogger(new ComponentIdentity("com.accidental-fish.application-support"));
            CancellationTokenSource source = new CancellationTokenSource();

            StartComponentHost(componentHost, logger, source);
            logger.Information("Something to log");
            Console.ReadLine();
            source.Cancel();
            Thread.Sleep(500);
        }

        private static void StartComponentHost(IComponentHost componentHost, ILogger logger, CancellationTokenSource cancellationTokenSource)
        {
            
            componentHost.Start(new StaticComponentHostConfigurationProvider(new List<ComponentConfiguration>
            {
                new ComponentConfiguration
                {
                    ComponentIdentity = AccidentalFish.ApplicationSupport.Processes.HostableComponentIdentities.LogQueueProcessor,
                    Instances = 1,
                    RestartEvaluator = (ex, retryCount) => RestartHandler(ex, retryCount, logger, AccidentalFish.ApplicationSupport.Processes.HostableComponentIdentities.LogQueueProcessor).Result
                }
            }), cancellationTokenSource);
        }

        private static async Task<bool> RestartHandler(Exception ex, int retryCount, ILogger logger, IComponentIdentity component)
        {
            try
            {
                bool doDelay = retryCount % 5 == 0;

                if (doDelay)
                {
                    await
                        logger.Warning(
                            $"Error occurred in component {component.FullyQualifiedName}. Restarting in 30 seconds.", ex);
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
                else
                {
                    await logger.Warning($"Error occurred in component {component.FullyQualifiedName}. Restarting immediately.", ex);
                }
            }
            catch (Exception)
            {
                // swallow any issues
            }
            return true;
        }
    }
}
