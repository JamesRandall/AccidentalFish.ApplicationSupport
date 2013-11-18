using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public abstract class AbstractHostableComponent : AbstractApplicationComponent, IHostableComponent
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        
        protected AbstractHostableComponent()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public Task Start()
        {
            return Task.Factory.StartNew(() => Activity, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        protected abstract Action<CancellationToken> Activity { get; }
    }
}
