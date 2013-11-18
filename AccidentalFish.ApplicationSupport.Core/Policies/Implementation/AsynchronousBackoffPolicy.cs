using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class AsynchronousBackoffPolicy : IAsynchronousBackoffPolicy, IDisposable
    {
        private static readonly List<int> BackoffTimings = new List<int> { 100, 250, 500, 1000, 5000 };
        private Action<Action<bool>> _workerAction;
        private Action _shutdownAction;
        private int _backoffIndex = -1;
        private CancellationToken _cancellationToken;
        private readonly ManualResetEventSlim _exitBackoffEvent = new ManualResetEventSlim(false);

        public void Execute(Action<Action<bool>> function, CancellationToken cancellationToken)
        {
            Execute(function, null, cancellationToken);
        }

        public void Execute(Action<Action<bool>> function, Action shutdownAction, CancellationToken cancellationToken)
        {
            _shutdownAction = shutdownAction;
            _cancellationToken = cancellationToken;
            _workerAction = function;
            _workerAction(Next);
            try
            {
                _exitBackoffEvent.Wait(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // cancelled    
            }
        }

        private async void Next(bool didWork)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                if (_shutdownAction != null)
                {
                    _shutdownAction();
                    _exitBackoffEvent.Set();
                }
                return;
            }

            if (!didWork)
            {
                int backoffIndex = Interlocked.Increment(ref _backoffIndex);
                if (backoffIndex >= BackoffTimings.Count)
                {
                    backoffIndex = BackoffTimings.Count - 1;
                }
                try
                {
                    await Task.Delay(BackoffTimings[backoffIndex], _cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    if (_shutdownAction != null)
                    {
                        _shutdownAction();
                        _exitBackoffEvent.Set();
                    }
                    return;
                }
                
                _workerAction(Next);
            }
            else
            {
                Interlocked.Exchange(ref _backoffIndex, -1);
                _workerAction(Next);
            }
        }

        public void Dispose()
        {
            _exitBackoffEvent.Dispose();
        }
    }
}
