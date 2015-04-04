using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Queues;
using CuttingEdge.Conditions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AsynchronousQueue<T> : IAsynchronousQueue<T> where T : class
    {
        private readonly CloudQueue _queue;
        private readonly IQueueSerializer _serializer;

        public AsynchronousQueue(IQueueSerializer queueSerializer, string connectionString, string queueName)
        {
            Condition.Requires(queueSerializer).IsNotNull();
            Condition.Requires(queueName).IsNotNullOrWhiteSpace();
            Condition.Requires(connectionString).IsNotNullOrWhiteSpace();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _queue = queueClient.GetQueueReference(queueName);
            _serializer = queueSerializer;
        }

        public Task EnqueueAsync(T item)
        {
            CloudQueueMessage message = new CloudQueueMessage(_serializer.Serialize(item));
            return _queue.AddMessageAsync(message);
        }

        public Task EnqueueAsync(T item, TimeSpan initialVisibilityDelay)
        {
            CloudQueueMessage message = new CloudQueueMessage(_serializer.Serialize(item));
            return _queue.AddMessageAsync(message, null, initialVisibilityDelay, null, null);
        }

        public async void Enqueue(T item, Action<T> success, Action<T, Exception> failure)
        {
            try
            {
                CloudQueueMessage message = new CloudQueueMessage(_serializer.Serialize(item));
                await _queue.AddMessageAsync(message);
                if (success != null) success(item);
            }
            catch (Exception ex)
            {
                if (failure != null) failure(item, ex);
            }
        }

        public Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor)
        {
            return DequeueAsync(processor, null);
        }

        public async Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor, TimeSpan? visibilityTimeout)
        {
            CloudQueueMessage message = await _queue.GetMessageAsync(visibilityTimeout, null, null);
            if (message != null)
            {
                T item = _serializer.Deserialize<T>(message.AsString);
                if (await processor(new CloudQueueItem<T>(message, item, message.DequeueCount, message.PopReceipt)))
                {
                    await _queue.DeleteMessageAsync(message);
                }
            }
            else
            {
                await processor(null);
            }
        }

        public async Task ExtendLeaseAsync(IQueueItem<T> queueItem, TimeSpan visibilityTimeout)
        {
            CloudQueueItem<T> queueItemImpl = queueItem as CloudQueueItem<T>;
            if (queueItemImpl == null)
            {
                throw new InvalidOperationException("Cannot mix Azure and non-Azure queue items when extending a lease");
            }
            await _queue.UpdateMessageAsync(queueItemImpl.CloudQueueMessage, visibilityTimeout, MessageUpdateFields.Visibility);
        }

        public void Dequeue(Func<IQueueItem<T>, bool> success, Action<Exception> failure)
        {
            Dequeue(success, null, failure);
        }
        
        public async void Dequeue(Func<IQueueItem<T>, bool> success, Action noMessageAction, Action<Exception> failure)
        {
            try
            {
                CloudQueueMessage message = await _queue.GetMessageAsync();
                if (message != null)
                {
                    T item = _serializer.Deserialize<T>(message.AsString);
                    if (success(new CloudQueueItem<T>(message, item, message.DequeueCount, message.PopReceipt)))
                    {
                        await _queue.DeleteMessageAsync(message);
                    }
                }
                else if (noMessageAction != null)
                {
                    noMessageAction();
                }
            }
            catch (Exception ex)
            {
                failure(ex);
            }
        }
    }
}
