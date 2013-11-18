using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Private;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.BackgroundProcesses
{
    internal class LogQueueProcessor : AbstractHostableComponent
    {
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IAsynchronousQueue<LogQueueItem> _queue;

        public LogQueueProcessor(IApplicationResourceFactory applicationResourceFactory)
        {
            _queue = applicationResourceFactory.GetLoggerQueue();
        }

        public LogQueueProcessor(IAsynchronousBackoffPolicy backoffPolicy)
        {
            _backoffPolicy = backoffPolicy;
        }

        protected override Action<CancellationToken> Activity
        {
            get
            {
                return (t) =>  _backoffPolicy.Execute(AttemptDequeue, t);
            }
        }

        private async void AttemptDequeue(Action<bool> backoffResultAction)
        {
            try
            {
                await _queue.DequeueAsync(item => ProcessItem(item, backoffResultAction));
            }
            catch (Exception)
            {
                Trace.TraceError("Unable to process queue item");
            }            
        }

        private async Task<bool> ProcessItem(LogQueueItem item, Action<bool> backoffResultAction)
        {
            if (item == null)
            {
                backoffResultAction(false);
                return await Task.FromResult(false);
            }

            // TODO: Spray into table store!
            return await Task.FromResult(false);
        }
    }
}
