using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class AsynchronousIntervalTimer : IAsynchronousIntervalTimer
    {
        private readonly IAsynchronousDelay _taskDelay;
        private readonly TimeSpan _interval;
        private readonly bool _delayOnExecute;

        public AsynchronousIntervalTimer(IAsynchronousDelay taskDelay, TimeSpan interval, bool delayOnExecute)
        {
            _taskDelay = taskDelay;
            _interval = interval;
            _delayOnExecute = delayOnExecute;
        }

        public async Task ExecuteAsync(Func<CancellationToken, Task<bool>> function, CancellationToken cancellationToken, Action shutdownAction=null)
        {
            if (_delayOnExecute)
            {
                await _taskDelay.Delay(_interval, cancellationToken);
            }
            bool shouldContinue = !cancellationToken.IsCancellationRequested;

            while (shouldContinue)
            {
                shouldContinue = await function(cancellationToken);
                if (shouldContinue)
                {
                    try
                    {
                        await _taskDelay.Delay(_interval, cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        
                    }
                    
                    shouldContinue = !cancellationToken.IsCancellationRequested;
                }                
            }

            shutdownAction?.Invoke();
        }

        public async Task ExecuteAsync(Func<Task<bool>> function, Action shutdownAction = null)
        {
            if (_delayOnExecute)
            {
                await _taskDelay.Delay(_interval);
            }
            bool shouldContinue = true;

            while (shouldContinue)
            {
                shouldContinue = await function();
                if (shouldContinue)
                {
                    await _taskDelay.Delay(_interval);
                }                
            }

            shutdownAction?.Invoke();
        }
    }
}
