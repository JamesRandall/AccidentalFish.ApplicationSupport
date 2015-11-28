using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AsynchronousBrokeredMessageQueue<T> : IAsynchronousQueue<T> where T : class
    {
        private readonly IQueueSerializer _queueSerializer;
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly IAzureAssemblyLogger _logger;
        private readonly QueueClient _client;

        public AsynchronousBrokeredMessageQueue(
            IQueueSerializer queueSerializer,
            string connectionString,
            string queueName,
            IAzureAssemblyLogger logger)
        {
            _queueSerializer = queueSerializer;
            _connectionString = connectionString;
            _queueName = queueName;
            _logger = logger;
            _client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            _logger?.Verbose("AsynchronousBrokeredMessageQueue: created for queue {0}", queueName);
        }

        public async Task EnqueueAsync(T item, IDictionary<string, object> messageProperties=null)
        {
            string value = _queueSerializer.Serialize(item);
            BrokeredMessage message = new BrokeredMessage(value);
            AddProperties(message, messageProperties);

            await _client.SendAsync(message);

            _logger?.Verbose("AsynchronousBrokeredMessageQueue: EnqueueAsync - enqueued item on queue {0}", _queueName);
        }

        public async Task EnqueueAsync(T item, TimeSpan initialVisibilityDelay, IDictionary<string, object> messageProperties = null)
        {
            string value = _queueSerializer.Serialize(item);
            BrokeredMessage message = new BrokeredMessage(value);
            AddProperties(message, messageProperties);
            message.ScheduledEnqueueTimeUtc = DateTimeOffset.UtcNow.Add(initialVisibilityDelay).DateTime;
            await _client.SendAsync(message);

            _logger?.Verbose("AsynchronousBrokeredMessageQueue: EnqueueAsync - enqueued item on queue {0} with delayed visibility of {1}ms", _queueName, initialVisibilityDelay.TotalMilliseconds);
        }

        public async Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> process)
        {
            BrokeredMessage message = await _client.ReceiveAsync();
            
            if (message != null)
            {
                _logger?.Verbose("AsynchronousBrokeredMessageQueue: DequeueAsync - dequeued item on queue {0}", _queueName);
                try
                {
                    string body = message.GetBody<string>();
                    T payload = _queueSerializer.Deserialize<T>(body);
                    BrokeredMessageQueueItem<T> queueItem = new BrokeredMessageQueueItem<T>(message, payload, message.DeliveryCount, null);
                    bool markComplete = await process(queueItem);
                    if (markComplete)
                    {
                        _logger?.Verbose("AsynchronousBrokeredMessageQueue: DequeueAsync - marking item complete on queue {0}", _queueName);
                        await message.CompleteAsync();
                    }
                    else
                    {
                        _logger?.Verbose("AsynchronousBrokeredMessageQueue: DequeueAsync - marking item abandoned on queue {0} at request of caller", _queueName);
                        await message.AbandonAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger?.Verbose("AsynchronousBrokeredMessageQueue: DequeueAsync - marking item abandoned on queue {0} due to error", ex, _queueName);
                    message.Abandon();
                }
            }
            else
            {
                // we pass null into the process function as it may still want to take action based on their being no message.
                await process(null);
            }
        }

        public Task ExtendLeaseAsync(IQueueItem<T> queueItem, TimeSpan visibilityTimeout)
        {
            _logger?.Verbose("AsynchronousBrokeredMessageQueue: ExtendLeaseAsync - extending a lease with a visibility timeout on queue {0}", _queueName);
            throw new NotSupportedException("Service Bus queues do not support specified visibility timeout extensions on lease extension. They extend by the default visibility in the queue definition. Please use the overloaded ExtendLeaseAsync method");
        }

        public async Task ExtendLeaseAsync(IQueueItem<T> queueItem)
        {
            BrokeredMessageQueueItem<T> brokeredMessageQueueItem = queueItem as BrokeredMessageQueueItem<T>;
            if (brokeredMessageQueueItem != null)
            {
                _logger?.Verbose("AsynchronousBrokeredMessageQueue: ExtendLeaseAsync - renewing lock on queue {0}", _queueName);
                await brokeredMessageQueueItem.Message.RenewLockAsync();
            }
            else
            {
                throw new InvalidOperationException("Not a brokered message queue item");
            }
        }

        private static void AddProperties(BrokeredMessage message, IDictionary<string, object> messageProperties)
        {
            if (messageProperties != null)
            {
                foreach (KeyValuePair<string, object> kvp in messageProperties)
                {
                    message.Properties.Add(kvp);
                }
            }
        }

        internal string ConnectionString => _connectionString;

        internal QueueClient UnderlyingQueue => _client;
    }
}
