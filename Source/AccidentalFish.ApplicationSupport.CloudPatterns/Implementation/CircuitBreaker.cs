using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.CloudPatterns.Implementation
{
    internal class CircuitBreaker : ICircuitBreaker
    {
        private readonly ICircuitBreakerStateStore _stateStore;
        private readonly TimeSpan _openToHalfOpenWaitTime;
        private readonly int _halfOpenSuccessResetThreshold;

        public CircuitBreaker(ICircuitBreakerStateStore stateStore) : this(stateStore, TimeSpan.FromSeconds(15), 10)
        {
            _stateStore = stateStore;
        }

        public CircuitBreaker(ICircuitBreakerStateStore stateStore, TimeSpan openToHalfOpenWaitTime, int halfOpenSuccessResetThreshold)
        {
            _stateStore = stateStore;
            _openToHalfOpenWaitTime = openToHalfOpenWaitTime;
            _halfOpenSuccessResetThreshold = halfOpenSuccessResetThreshold;
        }

        public CircuitBreakerStateEnum State
        {
            get { return _stateStore.State; }
        }

        public Exception LastException
        {
            get { return _stateStore.LastException; }
        }

        public DateTime LastStateChange
        {
            get { return _stateStore.LastChangedStateAt; }
        }

        public void Close()
        {
            _stateStore.Reset();
        }

        public void Open()
        {
            _stateStore.Trip(null);
        }

        public async void Execute(Action action)
        {
            await ExecuteAsync(() =>
            {
                action();
                return Task.FromResult(0);
            });
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            CircuitBreakerStateEnum state = _stateStore.State;

            if (state == CircuitBreakerStateEnum.Closed)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    _stateStore.Trip(ex);
                    throw;
                }
            }

            if (state == CircuitBreakerStateEnum.Open)
            {
                await ExecuteOpenAsync(action);
                return;
            }

            if (state == CircuitBreakerStateEnum.HalfOpen)
            {
                await ExecuteHalfOpenAsync(action, false);
            }
            
        }

        private async Task ExecuteOpenAsync(Func<Task> action)
        {
            if (_stateStore.LastChangedStateAt + _openToHalfOpenWaitTime < DateTime.UtcNow)
            {
                await ExecuteHalfOpenAsync(action, true);
            }
            else
            {
                throw new CircuitBreakerException("Circuit breaker is open");
            }
        }

        private async Task ExecuteHalfOpenAsync(Func<Task> action, bool setHalfOpenState)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_stateStore.HalfOpenSyncObject, ref lockTaken);
                if (lockTaken)
                {
                    if (setHalfOpenState)
                    {
                        _stateStore.HalfOpen();
                    }
                    await action();
                    if (_stateStore.IncrementHalfOpenSuccessfulAction() > _halfOpenSuccessResetThreshold)
                    {
                        _stateStore.Reset();
                    }
                }
                else
                {
                    throw new CircuitBreakerException("Circuit breaker is half open, action was not executed.");
                }
            }
            catch (CircuitBreakerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _stateStore.Trip(ex);
                throw;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_stateStore.HalfOpenSyncObject);
                }
            }
        }
    }
}
