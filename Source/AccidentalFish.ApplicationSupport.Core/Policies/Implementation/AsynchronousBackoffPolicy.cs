using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class AsynchronousBackoffPolicy : IAsynchronousBackoffPolicy
    {
        private static readonly List<int> BackoffTimings = new List<int> { 100, 250, 500, 1000, 5000 };
        private Action _shutdownAction;
        private int _backoffIndex = -1;
        private CancellationToken _cancellationToken;

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
            if (_shutdownAction != null)
            {
                _shutdownAction();
            }
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