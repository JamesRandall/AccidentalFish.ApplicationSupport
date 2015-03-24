using System;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Queues;
using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class QueueFactory : IQueueFactory
    {
        private readonly IConfiguration _configuration;

        public QueueFactory(IConfiguration configuration)
        {
            Condition.Requires(configuration).IsNotNull();
            _configuration = configuration;
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class
        {
            return new AsynchronousQueue<T>(new QueueSerializer<T>(), _configuration.StorageAccountConnectionString, queueName);
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            return new AsynchronousQueue<T>(new QueueSerializer<T>(), storageAccountConnectionString, queueName);
        }

        public IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string queueName) where T : class
        {
            return new AsynchronousBrokeredMessageQueue<T>(new QueueSerializer<T>(), _configuration.ServiceBusConnectionString, queueName);
        }

        public IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string serviceBusConnectionString, string queueName) where T : class
        {
            return new AsynchronousBrokeredMessageQueue<T>(new QueueSerializer<T>(), serviceBusConnectionString, queueName);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string topicName) where T : class
        {
            return new AsynchronousTopic<T>(new QueueSerializer<T>(), _configuration.StorageAccountConnectionString, topicName);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return new AsynchronousTopic<T>(new QueueSerializer<T>(), storageAccountConnectionString, topicName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName) where T : class
        {
            return new AsynchronousSubscription<T>(
                new QueueSerializer<T>(),
                _configuration.StorageAccountConnectionString,
                topicName,
                Guid.NewGuid().ToString().Replace("-", ""));
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName, string subscrioptionName) where T : class
        {
            return new AsynchronousSubscription<T>(
                new QueueSerializer<T>(),
                _configuration.StorageAccountConnectionString,
                topicName,
                subscrioptionName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName,
            string subscriptionName) where T : class
        {
            return new AsynchronousSubscription<T>(
                new QueueSerializer<T>(),
                storageAccountConnectionString,
                topicName,
                subscriptionName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return new AsynchronousSubscription<T>(
                new QueueSerializer<T>(),
                storageAccountConnectionString,
                topicName,
                Guid.NewGuid().ToString().Replace("-", ""));
        }
    }
}
