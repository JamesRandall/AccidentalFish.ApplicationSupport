using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.BackoffProcesses
{
    public abstract class BackoffQueueProcessor<T> : IHostableComponent where T : class
    {
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IAsynchronousQueue<T> _queue;
        private readonly ILogger _logger;

        private class ProcessResult
        {
            public bool Complete { get; set; }

            public bool DidWork { get; set; }
        }

        protected BackoffQueueProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousQueue<T> queue)
            : this(backoffPolicy, queue, null)
        {

        }

        protected BackoffQueueProcessor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IAsynchronousQueue<T> queue,
            ILogger logger)
        {
            _backoffPolicy = backoffPolicy;
            _queue = queue;
            _logger = logger;
        }

        protected ILogger Logger { get { return _logger; } }

        protected IAsynchronousQueue<T> Queue { get { return _queue; } }

        protected abstract Task<bool> HandleRecievedItem(IQueueItem<T> item);

        public abstract IComponentIdentity ComponentIdentity { get; }

        public async Task Start(CancellationToken token)
        {
            await _backoffPolicy.Execute(AttemptDequeue, token);
        }

        private async Task<bool> AttemptDequeue()
        {
            try
            {
                bool didWork = true;
                await _queue.DequeueAsync(async (item) =>
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
                Complete = true,
                DidWork = await HandleRecievedItem(message) // let the virtual method decide whether to complete (true) or abandon (false) the message
            };

            return result;
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