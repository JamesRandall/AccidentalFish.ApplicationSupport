using System;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class BrokeredMessageQueue<T> : IQueue<T> where T : class
    {
        private readonly IQueueSerializer _queueSerializer;
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly IAzureAssemblyLogger _logger;
        private readonly QueueClient _client;

        public BrokeredMessageQueue(IQueueSerializer queueSerializer, string connectionString, string queueName, IAzureAssemblyLogger logger)
        {
            _queueSerializer = queueSerializer;
            _connectionString = connectionString;
            _queueName = queueName;
            _logger = logger;
            _client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            _logger?.Verbose("BrokeredMessageQueue: created for queue {0}", queueName);
        }

        public void Enqueue(T item, Action<T> success, Action<T, Exception> failure)
        {
            try
            {
                _logger?.Verbose("BrokeredMessageQueue: Enqueue - enqueueing item on queue {0}", _queueName);
                string value = _queueSerializer.Serialize(item);
                BrokeredMessage message = new BrokeredMessage(value);
                _client.Send(message);
                success?.Invoke(item);
            }
            catch (Exception ex)
            {
                _logger?.Verbose("BrokeredMessageQueue: Enqueue - error occurred on queue {0}", ex, _queueName);
                failure?.Invoke(item, ex);
            }
        }

        public void Dequeue(Func<IQueueItem<T>, bool> success, Action<Exception> failure)
        {
            BrokeredMessage message = _client.Receive();

            if (message != null)
            {
                _logger?.Verbose("BrokeredMessageQueue: Dequeue - dequeued item from queue {0}", _queueName);
                try
                {
                    string body = message.GetBody<string>();
                    T payload = _queueSerializer.Deserialize<T>(body);
                    BrokeredMessageQueueItem<T> queueItem = new BrokeredMessageQueueItem<T>(message, payload, message.DeliveryCount, null);
                    bool markComplete = success(queueItem);
                    if (markComplete)
                    {
                        _logger?.Verbose("BrokeredMessageQueue: Dequeue - marking item complete on queue {0} at request of caller", _queueName);
                        message.Complete();
                    }
                    else
                    {
                        _logger?.Verbose("BrokeredMessageQueue: Dequeue - marking item abandoned on queue {0} at request of caller", _queueName);
                        message.Abandon();
                    }
                }
                catch (Exception ex)
                {
                    message.Abandon();
                    failure(ex);

                    _logger?.Verbose("BrokeredMessageQueue: Dequeue - error occurred and marking abandoned on queue {0}", ex, _queueName);
                }
            }
        }

        public void Dequeue(Func<IQueueItem<T>, bool> success, Action noMessageAction, Action<Exception> failure)
        {
            BrokeredMessage message = _client.Receive();

            if (message != null)
            {
                _logger?.Verbose("BrokeredMessageQueue: Dequeue - dequeued item from queue {0}", _queueName);
                try
                {
                    string body = message.GetBody<string>();
                    T payload = _queueSerializer.Deserialize<T>(body);
                    BrokeredMessageQueueItem<T> queueItem = new BrokeredMessageQueueItem<T>(message, payload, message.DeliveryCount, null);
                    bool markComplete = success(queueItem);
                    if (markComplete)
                    {
                        _logger?.Verbose("BrokeredMessageQueue: Dequeue - marking item complete on queue {0} at request of caller", _queueName);
                        message.Complete();
                    }
                    else
                    {
                        _logger?.Verbose("BrokeredMessageQueue: Dequeue - marking item abandoned on queue {0} at request of caller", _queueName);
                        message.Abandon();
                    }
                }
                catch (Exception ex)
                {
                    message.Abandon();
                    failure(ex);

                    _logger?.Verbose("BrokeredMessageQueue: Dequeue - error occurred and marking abandoned on queue {0}", ex, _queueName);
                }
            }
            else
            {
                noMessageAction();
            }
        }

        public void ExtendLease(IQueueItem<T> queueItem, TimeSpan visibilityTimeout)
        {
            _logger?.Verbose("BrokeredMessageQueue: ExtendLease - extending a lease with a visibility timeout on queue {0}", _queueName);
            throw new NotSupportedException("Service Bus queues do not support specified visibility timeout extensions on lease extension. They extend by the default visibility in the queue definition. Please use the overloaded ExtendLease method");
        }

        public void ExtendLease(IQueueItem<T> queueItem)
        {
            BrokeredMessageQueueItem<T> brokeredMessageQueueItem = queueItem as BrokeredMessageQueueItem<T>;
            if (brokeredMessageQueueItem != null)
            {
                _logger?.Verbose("BrokeredMessageQueue: ExtendLease - renewing lock on queue {0}", _queueName);
                brokeredMessageQueueItem.Message.RenewLock();
            }
            else
            {
                throw new InvalidOperationException("Not a brokered message queue item");
            }
        }

        internal string ConnectionString => _connectionString;

        internal QueueClient UnderlyingQueue => _client;
    }
}
