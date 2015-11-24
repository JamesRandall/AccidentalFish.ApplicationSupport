using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AsynchronousSubscription<T> : IAsynchronousSubscription<T> where T : class
    {
        private readonly IQueueSerializer _queueSerializer;
        private readonly string _connectionString;
        private readonly string _topicName;
        private readonly string _subscriptionName;
        private readonly IAzureAssemblyLogger _logger;
        private readonly SubscriptionClient _client;

        public AsynchronousSubscription(
            IQueueSerializer queueSerializer,
            string connectionString,
            string topicName,
            string subscriptionName,
            IAzureAssemblyLogger logger)
        {
            if (subscriptionName == null) throw new ArgumentNullException(nameof(subscriptionName));
            _queueSerializer = queueSerializer;
            _connectionString = connectionString;
            _topicName = topicName;
            _subscriptionName = subscriptionName;
            _logger = logger;
            _client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName, ReceiveMode.PeekLock);
            _logger?.Verbose("AsynchronousSubscription: created for topic {0} subscription {1}", topicName, subscriptionName);
        }

        public async Task<bool> RecieveAsync(Func<T, Task<bool>> process)
        {
            BrokeredMessage message = await _client.ReceiveAsync();
            if (message != null)
            {
                try
                {
                    _logger?.Verbose("AsynchronousSubscription: RecieveAsync - recieved item from topic {0} on subscription {1}", _topicName, _subscriptionName);
                    string body = message.GetBody<string>();
                    T payload = _queueSerializer.Deserialize<T>(body);
                    bool markComplete = await process(payload);
                    if (markComplete)
                    {
                        await message.CompleteAsync();
                        _logger?.Verbose("AsynchronousSubscription: RecieveAsync - marked item from topic {0} on subscription {1} as complete", _topicName, _subscriptionName);
                    }
                    else
                    {
                        await message.AbandonAsync();
                        _logger?.Verbose("AsynchronousSubscription: RecieveAsync - marked item from topic {0} on subscription {1} as abandoned at callers request", _topicName, _subscriptionName);
                    }
                }
                catch (Exception)
                {
                    message.Abandon();
                    _logger?.Verbose("AsynchronousSubscription: RecieveAsync - marked item from topic {0} on subscription {1} as abandoned due to error", _topicName, _subscriptionName);
                }
            }
            else
            {
                // we pass null into the process function as it may still want to take action based on their being no message.
                await process(null);
            }
            return message != null;
        }

        internal string ConnectionString => _connectionString;

        internal SubscriptionClient UnderlyingSubscription => _client;
    }
}
