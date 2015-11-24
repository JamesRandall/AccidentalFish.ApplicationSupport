using System;
using AccidentalFish.ApplicationSupport.Azure.Logging;
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
        private readonly string _queueName;
        private readonly IAzureAssemblyLogger _logger;

        public StorageQueue(IQueueSerializer queueSerializer, string connectionString, string queueName, IAzureAssemblyLogger logger)
        {
            if (queueSerializer == null) throw new ArgumentNullException(nameof(queueSerializer));
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentNullException(nameof(queueName));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _queue = queueClient.GetQueueReference(queueName);
            _serializer = queueSerializer;
            _queueName = queueName;
            _logger = logger;

            _logger?.Verbose("StorageQueue: created for queue {0}", queueName);
        }

        public async void Enqueue(T item, Action<T> success = null, Action<T, Exception> failure = null)
        {
            try
            {
                _logger?.Verbose("StorageQueue: Enqueue - enqueueing item on queue {0}", _queueName);
                CloudQueueMessage message = new CloudQueueMessage(_serializer.Serialize(item));
                await _queue.AddMessageAsync(message);
                success?.Invoke(item);
            }
            catch (Exception ex)
            {
                failure?.Invoke(item, ex);
                if (failure == null)
                {
                    throw;
                }
            }
        }

        public void Dequeue(Func<IQueueItem<T>, bool> success, Action<Exception> failure = null)
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
                    _logger?.Verbose("StorageQueue: Dequeue - dequeued item from queue {0}", _queueName);
                    T item = _serializer.Deserialize<T>(message.AsString);
                    if (success(new CloudQueueItem<T>(message, item, message.DequeueCount, message.PopReceipt)))
                    {
                        await _queue.DeleteMessageAsync(message);
                        _logger?.Verbose("StorageQueue: Dequeue - deleted item from queue {0}", _queueName);
                    }
                    else
                    {
                        _logger?.Verbose("StorageQueue: Dequeue - returning item to queue {0}", _queueName);
                    }
                }
                else
                {
                    noMessageAction?.Invoke();
                }
            }
            catch (Exception ex)
            {
                if (failure == null)
                {
                    throw;
                }
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
            _logger?.Verbose("StorageQueue: ExtendLease - extending by {0}ms", visibilityTimeout.TotalMilliseconds);
        }

        public void ExtendLease(IQueueItem<T> queueItem)
        {
            CloudQueueItem<T> queueItemImpl = queueItem as CloudQueueItem<T>;
            if (queueItemImpl == null)
            {
                throw new InvalidOperationException("Cannot mix Azure and non-Azure queue items when extending a lease");
            }
            _queue.UpdateMessage(queueItemImpl.CloudQueueMessage, TimeSpan.FromSeconds(30), MessageUpdateFields.Visibility);
            _logger?.Verbose("StorageQueue: ExtendLease - extending by {0}ms", TimeSpan.FromSeconds(30).TotalMilliseconds);
        }

        internal CloudQueue UnderlyingQueue => _queue;
    }
}
