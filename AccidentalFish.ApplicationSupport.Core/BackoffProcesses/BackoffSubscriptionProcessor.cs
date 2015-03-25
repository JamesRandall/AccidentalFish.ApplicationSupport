using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.BackoffProcesses
{
    public abstract class BackoffSubscriptionProcessor<T> : IHostableComponent where T : class
    {
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IAsynchronousSubscription<T> _subscription;
        private readonly ILogger _logger;

        protected BackoffSubscriptionProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousSubscription<T> subscription) : this(backoffPolicy, subscription, null)
        {
            
        }

        protected BackoffSubscriptionProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousSubscription<T> subscription,
            ILogger logger)
        {
            _backoffPolicy = backoffPolicy;
            _subscription = subscription;
            _logger = logger;
        }

        protected ILogger Logger { get { return _logger; } }

        protected IAsynchronousSubscription<T> Subscription { get {  return _subscription; } } 

        protected abstract Task<bool> HandleRecievedItem(T item);

        public abstract IComponentIdentity ComponentIdentity { get; }

        public async Task Start(CancellationToken token)
        {
            _backoffPolicy.Execute(AttemptDequeue, token);
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(500, token);
            }
        }

        private async void AttemptDequeue(Action<bool> backoffResultAction)
        {
            try
            {
                await _subscription.Recieve(item => ProcessItem(item, backoffResultAction));
            }
            catch (Exception ex)
            {
                LogError("Unable to dequeue message from queue", ex);
            }
        }

        private async Task<bool> ProcessItem(T message, Action<bool> backoffResultAction)
        {
            if (message == null)
            {
                backoffResultAction(false); // no item to process
                return false;
            }

            try
            {
                return await HandleRecievedItem(message); // let the virtual method decide whether to complete (true) or abandon (false) the message
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                backoffResultAction(true); // yes we processed an item
            }
        }

        private void LogError(string message, Exception ex)
        {
            if (_logger != null)
            {
                _logger.Error(message, ex).Wait();
            }
        }
    }
}
