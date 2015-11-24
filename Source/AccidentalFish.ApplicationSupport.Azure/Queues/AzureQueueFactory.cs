using System;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AzureQueueFactory : IAzureQueueFactory
    {
        private readonly IQueueFactory _queueFactory;
        private readonly IConfiguration _configuration;
        private readonly IQueueSerializer _queueSerializer;
        private readonly IAzureAssemblyLogger _logger;

        public AzureQueueFactory(IQueueFactory queueFactory,
            IConfiguration configuration,
            IQueueSerializer queueSerializer,
            IAzureAssemblyLogger logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (queueSerializer == null) throw new ArgumentNullException(nameof(queueSerializer));
            _queueFactory = queueFactory;
            _configuration = configuration;
            _queueSerializer = queueSerializer;
            _logger = logger;
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class
        {
            return _queueFactory.CreateAsynchronousQueue<T>(queueName);
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, queueName);
        }

        public IQueue<T> CreateQueue<T>(string queueName) where T : class
        {
            return _queueFactory.CreateQueue<T>(queueName);
        }

        public IQueue<T> CreateQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, queueName);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string topicName) where T : class
        {
            return _queueFactory.CreateAsynchronousTopic<T>(topicName);
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return _queueFactory.CreateAsynchronousTopic<T>(storageAccountConnectionString, topicName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName) where T : class
        {
            return _queueFactory.CreateAsynchronousSubscriptionWithConfiguration<T>(topicName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName, string subscriptionName) where T : class
        {
            return _queueFactory.CreateAsynchronousSubscriptionWithConfiguration<T>(topicName, subscriptionName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName,
            string subscriptionName) where T : class
        {
            return _queueFactory.CreateAsynchronousSubscription<T>(storageAccountConnectionString, topicName, subscriptionName);
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            return _queueFactory.CreateAsynchronousSubscription<T>(storageAccountConnectionString, topicName);
        }

        public IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string queueName) where T : class
        {
            return new AsynchronousBrokeredMessageQueue<T>(_queueSerializer, _configuration.ServiceBusConnectionString, queueName, _logger);
        }

        public IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string serviceBusConnectionString, string queueName) where T : class
        {
            return new AsynchronousBrokeredMessageQueue<T>(_queueSerializer, serviceBusConnectionString, queueName, _logger);
        }

        public IQueue<T> CreateBrokeredMessageQueue<T>(string queueName) where T : class
        {
            return new BrokeredMessageQueue<T>(_queueSerializer, _configuration.ServiceBusConnectionString, queueName, _logger);
        }

        public IQueue<T> CreateBrokeredMessageQueue<T>(string serviceBusConnectionString, string queueName) where T : class
        {
            return new BrokeredMessageQueue<T>(_queueSerializer, serviceBusConnectionString, queueName, _logger);
        }
    }
}
