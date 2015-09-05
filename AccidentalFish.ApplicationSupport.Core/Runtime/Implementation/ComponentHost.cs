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
        public const string FullyQualifiedName = "com.accidentalfish.application-support.component-host";
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger _logger;

        public ComponentHost(IComponentFactory componentFactory, ILoggerFactory loggerFactory)
        {
            _componentFactory = componentFactory;
            _logger = loggerFactory.CreateLongLivedLogger(ComponentIdentity);
        }

        public Action<Exception, int> CustomErrorHandler { get; set; }

        public async Task<IEnumerable<Task>> StartAsync(IComponentHostConfigurationProvider configurationProvider, CancellationTokenSource cancellationTokenSource)
        {
            IEnumerable<ComponentConfiguration> componentConfigurations = await configurationProvider.GetConfigurationAsync();
            _cancellationTokenSource = cancellationTokenSource;
            List<Task> tasks = new List<Task>();
            foreach (ComponentConfiguration componentConfiguration in componentConfigurations)
            {
                await _logger.Information(
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
            return Task.Factory.StartNew(() =>
            {
                int retryCount = 0;
                bool shouldRetry = true;
                while (shouldRetry)
                {
                    try
                    {
                        Task.Factory.StartNew(() =>
                        {
                            _logger.Information($"Hostable component {componentIdentity} is starting");
                            IHostableComponent component = _componentFactory.Create(componentIdentity);
                            component.StartAsync(_cancellationTokenSource.Token).Wait();
                            shouldRetry = false; // normal exit
                            _logger.Information($"Hostable component {componentIdentity} is exiting");
                        }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Wait();
                        shouldRetry = false;
                    }
                    catch (Exception ex)
                    {
                        CustomErrorHandler?.Invoke(ex, retryCount);

                        retryCount++;
                        shouldRetry = restartEvaluator != null && restartEvaluator(ex, retryCount);
                        if (shouldRetry)
                        {
                            _logger.Information($"Restarting {retryCount} for component {componentIdentity}", ex);
                            AggregateException exception = ex as AggregateException;
                            if (exception != null)
                            {
                                foreach (Exception innerException in exception.InnerExceptions)
                                {
                                    _logger.Error(Format("Aggregate error for component {1} on retry {0}", retryCount, componentIdentity), innerException);
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
                                    _logger.Error($"Component failure {retryCount} for component {componentIdentity}", innerException);
                                }
                            }
                            else
                            {
                                _logger.Error($"Component failure {retryCount} for component {componentIdentity}", ex);
                            }
                        }
                    }
                }
                
            }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
