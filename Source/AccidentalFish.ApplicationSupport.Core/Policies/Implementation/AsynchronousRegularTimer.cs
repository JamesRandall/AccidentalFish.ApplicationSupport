using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class AsynchronousRegularTimer : IAsynchronousRegularTimer
    {
        private readonly ITimerThreadPoolExecuter _threadPoolExecuter;
        private readonly IAsynchronousDelay _taskDelay;
        private readonly TimeSpan _interval;
        private readonly bool _delayOnExecute;

        public AsynchronousRegularTimer(ITimerThreadPoolExecuter threadPoolExecuter, IAsynchronousDelay taskDelay, TimeSpan interval, bool delayOnExecute)
        {
            _threadPoolExecuter = threadPoolExecuter;
            _taskDelay = taskDelay;
            _interval = interval;
            _delayOnExecute = delayOnExecute;
        }

        public async Task ExecuteAsync(Action<CancellationToken> action, CancellationToken cancellationToken, Action shutdownAction = null)
        {
            if (_delayOnExecute)
            {
                await _taskDelay.Delay(_interval, cancellationToken);
            }
            
            while (!cancellationToken.IsCancellationRequested)
            {
                using (CancellationTokenSource timeLimitedTask = new CancellationTokenSource(_interval))
                {
                    await Task.WhenAll(_threadPoolExecuter.Run(() => action(cancellationToken), timeLimitedTask.Token), _taskDelay.Delay(_interval, cancellationToken));
                }
            }

            shutdownAction?.Invoke();
        }        
    }
}
