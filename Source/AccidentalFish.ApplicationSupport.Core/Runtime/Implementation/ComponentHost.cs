using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using static System.String;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    [ComponentIdentity(FullyQualifiedName)]
    internal class ComponentHost : AbstractApplicationComponent, IComponentHost
    {
        private readonly IComponentFactory _componentFactory;
        private readonly IComponentHostRestartHandler _componentHostRestartHandler;
        public const string FullyQualifiedName = "com.accidentalfish.application-support.component-host";
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger _logger;

        public ComponentHost(IComponentFactory componentFactory, ILoggerFactory loggerFactory, IComponentHostRestartHandler componentHostRestartHandler)
        {
            _componentFactory = componentFactory;
            _componentHostRestartHandler = componentHostRestartHandler;
            _logger = loggerFactory.CreateLogger(ComponentIdentity);
        }

        public Action<Exception, int> CustomErrorHandler { get; set; }

        public async Task<IEnumerable<Task>> StartAsync(IComponentHostConfigurationProvider configurationProvider, CancellationTokenSource cancellationTokenSource)
        {
            IEnumerable<ComponentConfiguration> componentConfigurations = await configurationProvider.GetConfigurationAsync();
            _cancellationTokenSource = cancellationTokenSource;
            List<Task> tasks = new List<Task>();
            foreach (ComponentConfiguration componentConfiguration in componentConfigurations)
            {
                _logger?.Verbose(
                    $"Starting {componentConfiguration.Instances} instances of {componentConfiguration.ComponentIdentity}");
                for (int instance = 0; instance < componentConfiguration.Instances; instance++)
                {
                    tasks.Add(StartTask(componentConfiguration.ComponentIdentity, componentConfiguration.RestartEvaluator));
                }
            }
            return tasks;
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private Task StartTask(IComponentIdentity componentIdentity, Func<Exception, int, bool> restartEvaluator)
        {
            return Task.Run(async () =>
            {
                int retryCount = 0;
                bool shouldRetry = true;
                while (shouldRetry)
                {
                    try
                    {
                        await Task.Run(async () =>
                        {
                            _logger?.Verbose($"Hostable component {componentIdentity} is starting");
                            IHostableComponent component = _componentFactory.Create(componentIdentity);
                            await component.StartAsync(_cancellationTokenSource.Token);
                            shouldRetry = false; // normal exit
                            _logger?.Verbose($"Hostable component {componentIdentity} is exiting");
                        }, _cancellationTokenSource.Token);
                        shouldRetry = false;
                    }
                    catch (Exception ex)
                    {
                        CustomErrorHandler?.Invoke(ex, retryCount);

                        retryCount++;
                        if (restartEvaluator != null)
                        {
                            shouldRetry = restartEvaluator(ex, retryCount);
                        }
                        else
                        {
                            shouldRetry = await _componentHostRestartHandler.HandleRestart(ex, retryCount, _logger, componentIdentity);
                        }
                        shouldRetry = restartEvaluator != null && restartEvaluator(ex, retryCount);
                        if (shouldRetry)
                        {
                            _logger?.Information($"Restarting {retryCount} for component {componentIdentity}", ex);
                            AggregateException exception = ex as AggregateException;
                            if (exception != null)
                            {
                                foreach (Exception innerException in exception.InnerExceptions)
                                {
                                    _logger?.Error(Format("Aggregate error for component {1} on retry {0}", retryCount, componentIdentity), innerException);
                                }
                            }
                        }
                        else
                        {
                            AggregateException exception = ex as AggregateException;
                            if (exception != null)
                            {
                                foreach (Exception innerException in exception.InnerExceptions)
                                {
                                    _logger?.Error($"Component failure {retryCount} for component {componentIdentity}", innerException);
                                }
                            }
                            else
                            {
                                _logger?.Error($"Component failure {retryCount} for component {componentIdentity}", ex);
                            }
                        }
                    }
                }
                
            }, _cancellationTokenSource.Token);
        }
    }
}
