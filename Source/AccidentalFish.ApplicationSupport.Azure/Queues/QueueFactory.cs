using System;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class QueueFactory : IQueueFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IQueueSerializer _queueSerializer;

        public QueueFactory(IConfiguration configuration,
            IQueueSerializer queueSerializer)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (queueSerializer == null) throw new ArgumentNullException(nameof(queueSerializer));
            _configuration = configuration;
            _queueSerializer = queueSerializer;
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class
        {
            return new AsynchronousQueue<T>(_queueSerializer, _configuration.StorageAccountConnectionString, queueName);
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            return new AsynchronousQueue<T>(_queueSerializer, storageAccountConnectionString, queueName);
        }

        public IQueue<T> CreateQueue<T>(string queueName) where T : class
        {
            return new StorageQueue<T>(_queueSerializer, _configuration.StorageAccountConnectionString, queueName);
        }

        public IQueue<T> CreateQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            return new StorageQueue<T>(_queueSerializer, storageAccountConnectionString, queueName);
        }

        public IQueue<T> CreateBrokeredMessageQueue<T>(string queueName) where T : class
        {
            return new BrokeredMessageQueue<T>(_queueSerializer, _configuration.ServiceBusConnectionString, queueName);
        }

        public IQueue<T> CreateBrokeredMessageQueue<T>(string serviceBusConnectionString, string queueName) where T : class
        {
            return new BrokeredMessageQueue<T>(_queueSerializer, serviceBusConnectionString, queueName);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string topicName) where T : class
        {
            return new AsynchronousTopic<T>(_queueSerializer, _configuration.StorageAccountConnectionString, topicName);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return new AsynchronousTopic<T>(_queueSerializer, storageAccountConnectionString, topicName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                _configuration.StorageAccountConnectionString,
                topicName,
                Guid.NewGuid().ToString().Replace("-", ""));
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName, string subscrioptionName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                _configuration.StorageAccountConnectionString,
                topicName,
                subscrioptionName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName,
            string subscriptionName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                storageAccountConnectionString,
                topicName,
                subscriptionName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                storageAccountConnectionString,
                topicName,
                Guid.NewGuid().ToString().Replace("-", ""));
        }
    }
}
