using System;

namespace AccidentalFish.ApplicationSupport.Core.Queues.Implementation
{
    internal class NotSupportedQueueFactory : IQueueFactory
    {
        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName, IQueueSerializer queueSerializer) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName, IQueueSerializer queueSerializer) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueue<T> CreateQueue<T>(string queueName) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueue<T> CreateQueue<T>(string storageAccountConnectionString, string queueName) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueue<T> CreateQueue<T>(string queueName, IQueueSerializer queueSerializer) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueue<T> CreateQueue<T>(string storageAccountConnectionString, string queueName, IQueueSerializer queueSerializer) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string topicName) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName, string subscriptionName) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName,
            string subscriptionName) where T : class
        {
            throw new NotImplementedException();
        }

        public IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName) where T : class
        {
            throw new NotImplementedException();
        }
    }
}