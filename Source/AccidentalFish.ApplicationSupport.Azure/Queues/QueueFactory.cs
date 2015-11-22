using System;
using AccidentalFish.ApplicationSupport.Azure.Extensions;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class QueueFactory : IQueueFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IQueueSerializer _queueSerializer;
        private readonly ILogger _logger;

        public QueueFactory(IConfiguration configuration,
            IQueueSerializer queueSerializer,
            ILoggerFactory loggerFactory)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (queueSerializer == null) throw new ArgumentNullException(nameof(queueSerializer));
            _configuration = configuration;
            _queueSerializer = queueSerializer;
            _logger = loggerFactory.GetAssemblyLogger();
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class
        {
            return new AsynchronousQueue<T>(_queueSerializer, _configuration.StorageAccountConnectionString, queueName, _logger);
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            return new AsynchronousQueue<T>(_queueSerializer, storageAccountConnectionString, queueName, _logger);
        }

        public IQueue<T> CreateQueue<T>(string queueName) where T : class
        {
            return new StorageQueue<T>(_queueSerializer, _configuration.StorageAccountConnectionString, queueName, _logger);
        }

        public IQueue<T> CreateQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            return new StorageQueue<T>(_queueSerializer, storageAccountConnectionString, queueName, _logger);
        }

        public IQueue<T> CreateBrokeredMessageQueue<T>(string queueName) where T : class
        {
            return new BrokeredMessageQueue<T>(_queueSerializer, _configuration.ServiceBusConnectionString, queueName, _logger);
        }

        public IQueue<T> CreateBrokeredMessageQueue<T>(string serviceBusConnectionString, string queueName) where T : class
        {
            return new BrokeredMessageQueue<T>(_queueSerializer, serviceBusConnectionString, queueName, _logger);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string topicName) where T : class
        {
            return new AsynchronousTopic<T>(_queueSerializer, _configuration.StorageAccountConnectionString, topicName, _logger);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return new AsynchronousTopic<T>(_queueSerializer, storageAccountConnectionString, topicName, _logger);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                _configuration.StorageAccountConnectionString,
                topicName,
                Guid.NewGuid().ToString().Replace("-", ""),
                _logger);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName, string subscrioptionName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                _configuration.StorageAccountConnectionString,
                topicName,
                subscrioptionName,
                _logger);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName,
            string subscriptionName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                storageAccountConnectionString,
                topicName,
                subscriptionName, _logger);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return new AsynchronousSubscription<T>(
                _queueSerializer,
                storageAccountConnectionString,
                topicName,
                Guid.NewGuid().ToString().Replace("-", ""),
                _logger);
        }
    }
}
