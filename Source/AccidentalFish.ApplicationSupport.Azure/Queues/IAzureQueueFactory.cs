using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    public interface IAzureQueueFactory : IQueueFactory
    {
        IQueue<T> CreateBrokeredMessageQueue<T>(string queueName) where T : class;
        IQueue<T> CreateBrokeredMessageQueue<T>(string storageAccountConnectionString, string queueName) where T : class;
        IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string queueName) where T : class;
        IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string storageAccountConnectionString, string queueName) where T : class;        
    }
}
