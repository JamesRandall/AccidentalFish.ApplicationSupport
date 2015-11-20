using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Ninject;
using AccidentalFish.ApplicationSupport.Processes;
using Ninject;

namespace LoggerWithNinject
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel();
            IDependencyResolver dependencyResolver = new NinjectApplicationFrameworkDependencyResolver(kernel);

            dependencyResolver
                .UseCore()
                .UseAzure()
                .UseHostableProcesses();

            IComponentHost componentHost = dependencyResolver.Resolve<IComponentHost>();
            ILoggerFactory loggerFactory = dependencyResolver.Resolve<ILoggerFactory>();
            IAsynchronousLogger logger = loggerFactory.CreateAsynchronousLogger(new ComponentIdentity("com.accidental-fish.application-support"));
            CancellationTokenSource source = new CancellationTokenSource();

            StartComponentHost(componentHost, logger, source);
            logger.InformationAsync("Something to log");
            Console.ReadLine();
            source.Cancel();
            Thread.Sleep(500);
        }

        private static void StartComponentHost(IComponentHost componentHost, IAsynchronousLogger logger, CancellationTokenSource cancellationTokenSource)
        {

            componentHost.StartAsync(new StaticComponentHostConfigurationProvider(new List<ComponentConfiguration>
            {
                new ComponentConfiguration
                {
                    ComponentIdentity = HostableComponentIdentities.LogQueueProcessor,
                    Instances = 1,
                    RestartEvaluator = (ex, retryCount) => RestartHandler(ex, retryCount, logger, HostableComponentIdentities.LogQueueProcessor).Result
                }
            }), cancellationTokenSource);
        }

        private static async Task<bool> RestartHandler(Exception ex, int retryCount, IAsynchronousLogger logger, IComponentIdentity component)
        {
            try
            {
                bool doDelay = retryCount % 5 == 0;

                if (doDelay)
                {
                    await
                        logger.WarningAsync(
                            $"Error occurred in component {component.FullyQualifiedName}. Restarting in 30 seconds.", ex);
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
                else
                {
                    await logger.WarningAsync($"Error occurred in component {component.FullyQualifiedName}. Restarting immediately.", ex);
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
