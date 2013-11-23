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
    [ComponentIdentity(HostableComponentNames.LogQueueProcessor)]
    internal class LogQueueProcessor : AbstractApplicationComponent, IHostableComponent
    {
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IAsynchronousQueue<LogQueueItem> _queue;

        public LogQueueProcessor(IApplicationResourceFactory applicationResourceFactory, IAsynchronousBackoffPolicy backoffPolicy)
        {
            _queue = applicationResourceFactory.GetLoggerQueue();
            _backoffPolicy = backoffPolicy;
        }

        // TODO: This is a bleed through of the component host not being correct
        public async Task Start(CancellationToken cancellationToken)
        {
            _backoffPolicy.Execute(AttemptDequeue, cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500, cancellationToken);
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
            backoffResultAction(true);
            return await Task.FromResult(true);
        }
    }
}
