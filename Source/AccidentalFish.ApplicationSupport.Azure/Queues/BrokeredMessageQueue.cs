using System;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class BrokeredMessageQueue<T> : IQueue<T> where T : class
    {
        private readonly IQueueSerializer _queueSerializer;
        private readonly string _connectionString;
        private readonly QueueClient _client;

        public BrokeredMessageQueue(IQueueSerializer queueSerializer, string connectionString, string queueName, ILogger logger)
        {
            _queueSerializer = queueSerializer;
            _connectionString = connectionString;
            _client = QueueClient.CreateFromConnectionString(connectionString, queueName);
        }

        public void Enqueue(T item, Action<T> success, Action<T, Exception> failure)
        {
            try
            {
                string value = _queueSerializer.Serialize(item);
                BrokeredMessage message = new BrokeredMessage(value);
                _client.Send(message);
                if (success != null) success(item);
            }
            catch (Exception ex)
            {
                if (failure != null) failure(item, ex);
            }
        }

        public void Dequeue(Func<IQueueItem<T>, bool> success, Action<Exception> failure)
        {
            BrokeredMessage message = _client.Receive();

            if (message != null)
            {
                try
                {
                    string body = message.GetBody<string>();
                    T payload = _queueSerializer.Deserialize<T>(body);
                    BrokeredMessageQueueItem<T> queueItem = new BrokeredMessageQueueItem<T>(message, payload, message.DeliveryCount, null);
                    bool markComplete = success(queueItem);
                    if (markComplete)
                    {
                        message.Complete();
                    }
                    else
                    {
                        message.Abandon();
                    }
                }
                catch (Exception ex)
                {
                    message.Abandon();
                    failure(ex);
                }
            }
        }

        public void Dequeue(Func<IQueueItem<T>, bool> success, Action noMessageAction, Action<Exception> failure)
        {
            BrokeredMessage message = _client.Receive();

            if (message != null)
            {
                try
                {
                    string body = message.GetBody<string>();
                    T payload = _queueSerializer.Deserialize<T>(body);
                    BrokeredMessageQueueItem<T> queueItem = new BrokeredMessageQueueItem<T>(message, payload, message.DeliveryCount, null);
                    bool markComplete = success(queueItem);
                    if (markComplete)
                    {
                        message.Complete();
                    }
                    else
                    {
                        message.Abandon();
                    }
                }
                catch (Exception ex)
                {
                    message.Abandon();
                    failure(ex);
                }
            }
            else
            {
                noMessageAction();
            }
        }

        public void ExtendLease(IQueueItem<T> queueItem, TimeSpan visibilityTimeout)
        {
            throw new NotSupportedException("Service Bus queues do not support specified visibility timeout extensions on lease extension. They extend by the default visibility in the queue definition. Please use the overloaded ExtendLease method");
        }

        public void ExtendLease(IQueueItem<T> queueItem)
        {
            BrokeredMessageQueueItem<T> brokeredMessageQueueItem = queueItem as BrokeredMessageQueueItem<T>;
            if (brokeredMessageQueueItem != null)
            {
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
