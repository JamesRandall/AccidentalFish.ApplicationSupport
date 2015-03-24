using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AsynchronousSubscription<T> : IAsynchronousSubscription<T> where T : class
    {
        private readonly IQueueSerializer<T> _queueSerializer;
        private readonly SubscriptionClient _client;

        public AsynchronousSubscription(IQueueSerializer<T> queueSerializer, string connectionString, string topicName, string subscriptionName)
        {
            _queueSerializer = queueSerializer;
            _client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName, ReceiveMode.PeekLock);
        }

        public async Task<bool> Recieve(Func<T, Task<bool>> process)
        {
            BrokeredMessage message = await _client.ReceiveAsync();
            if (message != null)
            {
                try
                {
                    string body = message.GetBody<string>();
                    T payload = _queueSerializer.Deserialize(body);
                    bool markComplete = await process(payload);
                    if (markComplete)
                    {
                        await message.CompleteAsync();
                    }
                    else
                    {
                        await message.AbandonAsync();
                    }
                }
                catch (Exception)
                {
                    message.Abandon();
                }
            }
            else
            {
                // we pass null into the process function as it may still want to take action based on their being no message.
                await process(null);
            }
            return message != null;
        }
    }
}
