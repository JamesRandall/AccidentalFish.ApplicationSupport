namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Factory for creating configured queues
    /// </summary>
    public interface IQueueFactory
    {
        /// <summary>
        /// Create an asynchronous queue with the given name with a connection string as specified in an app setting of azure-storage-connection-string
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="queueName">The name of the queue</param>
        /// <returns>A configured queue</returns>
        IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string queueName) where T : class;

        /// <summary>
        /// Create an asynchronous queue with the given name with a connection string in the specified storage account
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="storageAccountConnectionString">Storage account for the queue</param>
        /// <param name="queueName">The name of the queue</param>
        /// <returns>A configured queue</returns>
        IAsynchronousQueue<T> CreateAsynchronousQueue<T>(string storageAccountConnectionString, string queueName) where T : class;

        /// <summary>
        /// Create an queue with the given name with a connection string as specified in an app setting of azure-storage-connection-string
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="queueName">The name of the queue</param>
        /// <returns>A configured queue</returns>
        IQueue<T> CreateQueue<T>(string queueName) where T : class;

        /// <summary>
        /// Create an  queue with the given name with a connection string in the specified storage account
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="storageAccountConnectionString">Storage account for the queue</param>
        /// <param name="queueName">The name of the queue</param>
        /// <returns>A configured queue</returns>
        IQueue<T> CreateQueue<T>(string storageAccountConnectionString, string queueName) where T : class;

        /// <summary>
        /// Create an asynchronous topic with the given name with a connection string as specified in an app setting of azure-storage-connection-string
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="topicName">The name of the topic</param>
        /// <returns>A configured topic</returns>
        IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string topicName) where T : class;

        /// <summary>
        /// Create an asynchronous topic with the given name with a connection string in the specified storage account
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="storageAccountConnectionString">Storage account for the queue</param>
        /// <param name="topicName">The name of the topic</param>
        /// <returns>A configured topic</returns>
        IAsynchronousTopic<T> CreateAsynchronousTopic<T>(string storageAccountConnectionString, string topicName) where T : class;

        /// <summary>
        /// Create an asynchronous subscription looking at the specified topic with the given name with a connection string as specified in an app setting of azure-storage-connection-string.
        /// The subscription is given an auto-generated name.
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="topicName">The name of the topic</param>
        /// <returns>A configured subscription</returns>
        IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName) where T : class;

        /// <summary>
        /// Create an asynchronous subscription looking at the specified topic with the given name with a connection string as specified in an app setting of azure-storage-connection-string.
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="topicName">The name of the topic</param>
        /// <param name="subscriptionName">The name of the subscription</param>
        /// <returns>A configured subscription</returns>
        IAsynchronousSubscription<T> CreateAsynchronousSubscriptionWithConfiguration<T>(string topicName, string subscriptionName) where T : class;

        /// <summary>
        /// Create an asynchronous subscription looking at the specified topic with the given name with a connection string.
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="storageAccountConnectionString">The connection string</param>
        /// <param name="topicName">The name of the topic</param>
        /// <param name="subscriptionName">The name of the subscription</param>
        /// <returns>A configured subscription</returns>
        IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName, string subscriptionName) where T : class;
        /// <summary>
        /// Create an asynchronous subscription looking at the specified topic with the given name with a connection string.
        /// The subscription is given an auto-generated name.
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="storageAccountConnectionString">The connection string</param>
        /// <param name="topicName">The name of the topic</param>
        /// <returns>A configured subscription</returns>
        IAsynchronousSubscription<T> CreateAsynchronousSubscription<T>(string storageAccountConnectionString, string topicName) where T : class;
    }
}
