using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.BackoffProcesses
{
    /// <summary>
    /// Base hostable component for processing items on a subscription falling away into a backoff pattern when no queue items are available.
    /// To implement basic subscription processing simply inherit from this class and override HandleRecievedItem supplying a back off policy
    /// and the subscription to the constructor.
    /// </summary>
    /// <typeparam name="T">The type of the queue item</typeparam>
    public abstract class BackoffSubscriptionProcessor<T> : IHostableComponent where T : class
    {
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IAsynchronousSubscription<T> _subscription;
        private readonly ILogger _logger;

        private class ProcessResult
        {
            public bool Complete { get; set; }

            public bool DidWork { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="backoffPolicy">The back off policy to use.</param>
        /// <param name="subscription">The subscription to be processed</param>
        protected BackoffSubscriptionProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousSubscription<T> subscription) : this(backoffPolicy, subscription, null)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="backoffPolicy">The back off policy to use.</param>
        /// <param name="subscription">The suubscription to be processed</param>
        /// <param name="logger">The logger to use for reporting issues</param>
        protected BackoffSubscriptionProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousSubscription<T> subscription,
            ILogger logger)
        {
            _backoffPolicy = backoffPolicy;
            _subscription = subscription;
            _logger = logger;
        }

        /// <summary>
        /// The logger the processor is configured with, may be null
        /// </summary>
        protected ILogger Logger => _logger;

        /// <summary>
        /// The subscription the processor is configured with
        /// </summary>
        protected IAsynchronousSubscription<T> Subscription => _subscription;

        /// <summary>
        /// Override to process a subscription item. 
        /// </summary>
        /// <param name="item">The subscription item to process</param>
        /// <returns>Return true to remove the item from the queue, false to return it to the queue.</returns>
        protected abstract Task<bool> HandleRecievedItemAsync(T item);

        /// <summary>
        /// The component identity - required by the component host
        /// </summary>
        public abstract IComponentIdentity ComponentIdentity { get; }

        public async Task StartAsync(CancellationToken token)
        {
            await _backoffPolicy.Execute(AttemptDequeue, token);
        }

        private async Task<bool> AttemptDequeue()
        {
            try
            {
                bool didWork = true;
                await _subscription.Recieve(async item =>
                {
                    ProcessResult result = await ProcessItem(item);
                    didWork = result.DidWork;
                    return result.Complete;
                });
                return didWork;
            }
            catch (Exception ex)
            {
                LogError("Unable to dequeue message from queue", ex);
                throw;
            }
        }

        private async Task<ProcessResult> ProcessItem(T message)
        {
            if (message == null)
            {
                return new ProcessResult
                {
                    Complete = false,
                    DidWork = false
                };
            }

            ProcessResult result = new ProcessResult
            {
                Complete = true,
                DidWork = await  HandleRecievedItemAsync((message))
            };

            return result;
        }

        private void LogError(string message, Exception ex)
        {
            _logger?.Error(message, ex).Wait();
        }
    }
}
