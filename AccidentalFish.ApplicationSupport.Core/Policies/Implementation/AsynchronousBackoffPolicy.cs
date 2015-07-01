using System;
using System.Collections.Generic;
using System.Net.Configuration;
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
        
        public async Task Execute(Func<Task<bool>> function, CancellationToken cancellationToken)
        {
            await Execute(function, null, cancellationToken);
        }

        public async Task Execute(Func<Task<bool>> function, Action shutdownAction, CancellationToken cancellationToken)
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
            } while (shouldContinue);
            if (_shutdownAction != null)
            {
                _shutdownAction();
            }
        }

        private async Task<bool> Backoff()
        {
            
            int backoffIndex = Interlocked.Increment(ref _backoffIndex);
            if (backoffIndex >= BackoffTimings.Count)
            {
                backoffIndex = BackoffTimings.Count - 1;
            }
            try
            {
                await Task.Delay(BackoffTimings[backoffIndex], _cancellationToken);
                return true;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }
    }
}
