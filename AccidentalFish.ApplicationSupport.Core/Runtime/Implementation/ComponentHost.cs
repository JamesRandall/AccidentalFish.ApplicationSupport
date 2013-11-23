using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using Microsoft.Practices.Unity;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    [ComponentIdentity(FullyQualifiedName)]
    internal class ComponentHost : AbstractApplicationComponent, IComponentHost
    {
        public const string FullyQualifiedName = "com.accidentalfish.application-support.component-host";
        private readonly IUnityContainer _unityContainer;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger _logger;

        public ComponentHost(IUnityContainer unityContainer, ILoggerFactory loggerFactory)
        {
            _unityContainer = unityContainer;
            _logger = loggerFactory.CreateLongLivedLogger(ComponentIdentity);
        }

        public async void Start(IComponentHostConfigurationProvider configurationProvider, CancellationTokenSource cancellationTokenSource)
        {
            IEnumerable<ComponentConfiguration> componentConfigurations = await configurationProvider.GetConfiguration();
            _cancellationTokenSource = cancellationTokenSource;
            foreach (ComponentConfiguration componentConfiguration in componentConfigurations)
            {
                _logger.Information(String.Format("Starting {0} instances of {1}", componentConfiguration.Instances, componentConfiguration.ComponentIdentity));
                for (int instance = 0; instance < componentConfiguration.Instances; instance++)
                {
                    StartTask(componentConfiguration.ComponentIdentity, componentConfiguration.RestartEvaluator);
                }
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        // TODO: Needs attention as this isn't right yet - too tired!
        private void StartTask(IComponentIdentity componentIdentity, Func<Exception, int, bool> restartEvaluator)
        {
            Task.Factory.StartNew(async () =>
            {
                int retryCount = 0;
                bool shouldRetry = true;
                while (shouldRetry)
                {
                    try
                    {
                        await Task.Factory.StartNew(async () =>
                        {
                            IHostableComponent component = _unityContainer.Resolve<IHostableComponent>(componentIdentity.ToString());
                            await component.Start(_cancellationTokenSource.Token);
                            _logger.Information(String.Format("Component {0} is exiting", componentIdentity));
                        }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        shouldRetry = restartEvaluator != null && restartEvaluator(ex, retryCount);
                        if (shouldRetry)
                        {
                            _logger.Information(String.Format("Restarting {0} for component {1}", retryCount, componentIdentity));
                        }
                        else
                        {
                            _logger.Error(String.Format("Component failure {0} for component {1}", retryCount, componentIdentity));
                        }
                    }
                }
                
            }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
