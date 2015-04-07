using System;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class BrokeredMessageQueue<T> : IQueue<T> where T : class
    {
        private readonly IQueueSerializer _queueSerializer;
        private readonly QueueClient _client;

        public BrokeredMessageQueue(
            IQueueSerializer queueSerializer,
            string connectionString,
            string queueName)
        {
            _queueSerializer = queueSerializer;
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
    }
}
