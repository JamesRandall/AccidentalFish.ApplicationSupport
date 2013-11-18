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
    }
}
