using System;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class StorageQueue<T> : IQueue<T> where T : class
    {
        private readonly CloudQueue _queue;
        private readonly IQueueSerializer _serializer;

        public StorageQueue(IQueueSerializer queueSerializer, string connectionString, string queueName)
        {
            if (queueSerializer == null) throw new ArgumentNullException(nameof(queueSerializer));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (String.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException(nameof(queueName));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _queue = queueClient.GetQueueReference(queueName);
            _serializer = queueSerializer;
        }

        public async void Enqueue(T item, Action<T> success, Action<T, Exception> failure)
        {
            try
            {
                CloudQueueMessage message = new CloudQueueMessage(_serializer.Serialize(item));
                await _queue.AddMessageAsync(message);
                success?.Invoke(item);
            }
            catch (Exception ex)
            {
                failure?.Invoke(item, ex);
            }
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
                else
                {
                    noMessageAction?.Invoke();
                }
            }
            catch (Exception ex)
            {
                failure(ex);
            }
        }

        public void ExtendLease(IQueueItem<T> queueItem, TimeSpan visibilityTimeout)
        {
            CloudQueueItem<T> queueItemImpl = queueItem as CloudQueueItem<T>;
            if (queueItemImpl == null)
            {
                throw new InvalidOperationException("Cannot mix Azure and non-Azure queue items when extending a lease");
            }
            _queue.UpdateMessage(queueItemImpl.CloudQueueMessage, visibilityTimeout, MessageUpdateFields.Visibility);
        }

        public void ExtendLease(IQueueItem<T> queueItem)
        {
            CloudQueueItem<T> queueItemImpl = queueItem as CloudQueueItem<T>;
            if (queueItemImpl == null)
            {
                throw new InvalidOperationException("Cannot mix Azure and non-Azure queue items when extending a lease");
            }
            _queue.UpdateMessage(queueItemImpl.CloudQueueMessage, TimeSpan.FromSeconds(30), MessageUpdateFields.Visibility);
        }

        internal CloudQueue UnderlyingQueue => _queue;
    }
}
