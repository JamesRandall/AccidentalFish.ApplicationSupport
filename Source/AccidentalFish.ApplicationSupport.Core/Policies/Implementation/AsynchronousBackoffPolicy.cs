using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class AsynchronousBackoffPolicy : IAsynchronousBackoffPolicy
    {
        private Action _shutdownAction;
        private int _backoffIndex = -1;
        private CancellationToken _cancellationToken;
        private readonly ICoreAssemblyLogger _logger;
        private readonly TimeSpan[] _backoffTimings;

        public AsynchronousBackoffPolicy(ICoreAssemblyLogger logger, IBackoffPolicyTimingProvider provider)
        {
            _logger = logger;
            _backoffTimings = provider.GetIntervals().ToArray();
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
            if (_backoffIndex >= _backoffTimings.Length)
            {
                _backoffIndex = _backoffTimings.Length - 1;
            }
            try
            {
                _logger?.Verbose("AsynchronousBackoffPolicy - backing off for {0}ms", _backoffTimings[_backoffIndex].TotalMilliseconds);
                await Task.Delay(_backoffTimings[_backoffIndex], _cancellationToken);
                return true;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }
    }
}