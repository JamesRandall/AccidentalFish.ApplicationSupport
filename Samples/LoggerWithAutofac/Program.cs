using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Autofac;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Processes;
using Autofac;

namespace LoggerWithAutofac
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            var dependencyResolver = new LazyAutofacApplicationFrameworkDependencyResolver(builder);

            dependencyResolver
                .UseCore()
                .UseAzure()
                .UseHostableProcesses();

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
