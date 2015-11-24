using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Extensions;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class AsynchronousBackoffPolicy : IAsynchronousBackoffPolicy
    {
        private static readonly List<int> BackoffTimings = new List<int> { 100, 250, 500, 1000, 5000 };
        private Action _shutdownAction;
        private int _backoffIndex = -1;
        private CancellationToken _cancellationToken;
        private readonly ICoreAssemblyLogger _logger;

        public AsynchronousBackoffPolicy(ICoreAssemblyLogger logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(Func<Task<bool>> function, CancellationToken cancellationToken)
        {
            await ExecuteAsync(function, null, cancellationToken);
        }

        public async Task ExecuteAsync(Func<Task<bool>> function, Action shutdownAction, CancellationToken cancellationToken)
        {
            _shutdownAction = shutdownAction;
            _cancellationToken = cancellationToken;

            bool shouldContinue = true;

            do
            {
                bool didWork = await function();
                if (!didWork)
                {
                    shouldContinue = await Backoff();
                }
                else
                {
                    _backoffIndex = -1;
                }

                if (_cancellationToken.IsCancellationRequested)
                {
                    shouldContinue = false;
                }
            } while (shouldContinue);
            _shutdownAction?.Invoke();
        }

        private async Task<bool> Backoff()
        {

            _backoffIndex++;
            if (_backoffIndex >= BackoffTimings.Count)
            {
                _backoffIndex = BackoffTimings.Count - 1;
            }
            try
            {
                _logger?.Verbose("AsynchronousBackoffPolicy - backing off for {0}ms", BackoffTimings[_backoffIndex]);
                await Task.Delay(BackoffTimings[_backoffIndex], _cancellationToken);
                return true;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }
    }
}