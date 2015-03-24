namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IQueueFactory
    {
        IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class;
        IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class;
        IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string queueName) where T : class;
        IAsynchronousQueue<T> CreateAsynchronousBrokeredMessageQueue<T>(string storageAccountConnectionString, string queueName) where T : class;
        IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string topicName) where T : class;
        IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string storageAccountConnectionString, string topicName) where T : class;
        IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName) where T : class;
        IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName, string subscrioptionName) where T : class;
        IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName, string subscriptionName) where T : class;
        IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName) where T : class;
    }
}
