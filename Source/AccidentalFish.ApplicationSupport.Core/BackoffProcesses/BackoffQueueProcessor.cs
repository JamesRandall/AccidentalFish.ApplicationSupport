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
    /// Base hostable component for processing items on a queue falling away into a backoff pattern when no queue items are available.
    /// To implement basic queue processing simply inherit from this class and override HandleRecievedItem supplying a back off policy
    /// and the queue to the constructor.
    /// </summary>
    /// <typeparam name="T">The type of the queue item</typeparam>
    public abstract class BackoffQueueProcessor<T> : IHostableComponent where T : class
    {
        private readonly Func<Exception, Task<bool>> _dequeErrorHandler;
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IAsynchronousQueue<T> _queue;
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
        /// <param name="queue">The queue to be processed</param>
        /// <param name="dequeErrorHandler">Optional error handler for dequeue failures. This can return true / false or throw an exception</param>
        protected BackoffQueueProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousQueue<T> queue,
            Func<Exception, Task<bool>> dequeErrorHandler=null) : this(backoffPolicy, queue, null, dequeErrorHandler)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="backoffPolicy">The back off policy to use.</param>
        /// <param name="queue">The queue to be processed</param>
        /// <param name="logger">The logger to use for reporting issues</param>
        /// <param name="dequeErrorHandler">Optional error handler for dequeue failures. This can return true / false or throw an exception</param>
        protected BackoffQueueProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousQueue<T> queue,
            ILogger logger,
            Func<Exception, Task<bool>> dequeErrorHandler = null)
        {
            _backoffPolicy = backoffPolicy;
            _queue = queue;
            _logger = logger;
            _dequeErrorHandler = dequeErrorHandler;
        }

        /// <summary>
        /// The logger the processor is configured with, may be null
        /// </summary>
        protected ILogger Logger => _logger;

        /// <summary>
        /// The queue the processor is configured with
        /// </summary>
        protected IAsynchronousQueue<T> Queue => _queue;

        /// <summary>
        /// Override to process a queue item. 
        /// </summary>
        /// <param name="item">The queue item to process</param>
        /// <returns>Return true to remove the item from the queue, false to return it to the queue.</returns>
        protected abstract Task<bool> HandleRecievedItemAsync(IQueueItem<T> item);

        /// <summary>
        /// The component identity - required by the component host
        /// </summary>
        public abstract IComponentIdentity ComponentIdentity { get; }

        /// <summary>
        /// Starts the component and begins queue processing.
        /// </summary>
        /// <param name="token">The cancellation token to use to indicate termination</param>
        /// <returns>An awaitable task</returns>
        public async Task StartAsync(CancellationToken token)
        {
            Logger?.Verbose("Starting BackoffQueueProcessor {0}", ComponentIdentity);
            await _backoffPolicy.ExecuteAsync(AttemptDequeueAsync, token);
            Logger?.Verbose("Exiting BackoffQueueProcessor {0}", ComponentIdentity);
        }

        private async Task<bool> AttemptDequeueAsync()
        {
            try
            {
                Logger?.Verbose("Attempting dequeue in {0}", ComponentIdentity);
                bool didWork = true;
                try
                {
                    await _queue.DequeueAsync(async item =>
                    {
                        ProcessResult result = await ProcessItem(item);
                        didWork = result.DidWork;
                        Logger?.Verbose("Dequeue result in {0} of {1}", ComponentIdentity, result);
                        return result.Complete;
                    });
                    return didWork;
                }
                catch (Exception ex)
                {
                    if (_dequeErrorHandler != null)
                    {
                        return await _dequeErrorHandler(ex);
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                Logger?.Error("Error occurred in dequeue in {0}", ex, ComponentIdentity);
                throw;
            }
        }

        private async Task<ProcessResult> ProcessItem(IQueueItem<T> message)
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
                Complete = await HandleRecievedItemAsync(message), // let the virtual method decide whether to complete (true) or abandon (false) the message,
                DidWork = true
            };

            return result;
        }
    }
}
