namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IQueueFactory
    {
        IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class;
        IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class;
    }
}
