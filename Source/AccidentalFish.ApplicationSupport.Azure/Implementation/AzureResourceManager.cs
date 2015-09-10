using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Queues;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Implementation
{
    internal class AzureResourceManager : IAzureResourceManager
    {
        public async Task CreateIfNotExistsAsync<T>(IAsynchronousQueue<T> abstractQueue) where T : class
        {
            AsynchronousQueue<T> storageQueue = abstractQueue as AsynchronousQueue<T>;
            if (storageQueue != null)
            {
                await storageQueue.UnderlyingQueue.CreateIfNotExistsAsync();
                return;
            }
            AsynchronousBrokeredMessageQueue<T> serviceBusQueue = abstractQueue as AsynchronousBrokeredMessageQueue<T>;
            if (serviceBusQueue != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusQueue.ConnectionString);
                if (!(await namespaceManager.QueueExistsAsync(serviceBusQueue.UnderlyingQueue.Path)))
                {
                    await namespaceManager.CreateQueueAsync(serviceBusQueue.UnderlyingQueue.Path);
                }
                return;
            }

            throw new ArgumentException($"The queue type {abstractQueue.GetType().FullName} is not supported for resource management");
        }

        public void CreateIfNotExists<T>(IQueue<T> abstractQueue) where T : class
        {
            StorageQueue<T> storageQueue = abstractQueue as StorageQueue<T>;
            if (storageQueue != null)
            {
                storageQueue.UnderlyingQueue.CreateIfNotExists();
                return;
            }
            BrokeredMessageQueue<T> serviceBusQueue = abstractQueue as BrokeredMessageQueue<T>;
            if (serviceBusQueue != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusQueue.ConnectionString);
                if (!namespaceManager.QueueExists(serviceBusQueue.UnderlyingQueue.Path))
                {
                    namespaceManager.CreateQueue(serviceBusQueue.UnderlyingQueue.Path);
                }
                return;
            }
            throw new ArgumentException($"The queue type {abstractQueue.GetType().FullName} is not supported for resource management");
        }

        public async Task CreateIfNotExistsAsync<T>(IAsynchronousTopic<T> abstractTopic) where T : class
        {
            AsynchronousTopic<T> topic = abstractTopic as AsynchronousTopic<T>;
            if (topic != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(topic.ConnectionString);
                if (!(await namespaceManager.QueueExistsAsync(topic.UnderlyingTopic.Path)))
                {
                    await namespaceManager.CreateTopicAsync(topic.UnderlyingTopic.Path);
                }
                return;
            }
            throw new ArgumentException($"The topic type {abstractTopic.GetType().FullName} is not supported for resource management");
        }

        public async Task CreateIfNotExistsAsync<T>(IAsynchronousSubscription<T> abstractSubscription) where T : class
        {
            AsynchronousSubscription<T> subscription = abstractSubscription as AsynchronousSubscription<T>;
            if (subscription != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(subscription.ConnectionString);
                if (!(await namespaceManager.QueueExistsAsync(subscription.UnderlyingSubscription.Name)))
                {
                    await namespaceManager.CreateSubscriptionAsync(subscription.UnderlyingSubscription.TopicPath, subscription.UnderlyingSubscription.Name);
                }
                return;
            }
            throw new ArgumentException($"The topic type {abstractSubscription.GetType().FullName} is not supported for resource management");
        }

        public async Task CreateIfNotExistsAsync<T>(IAsynchronousTableStorageRepository<T> abstractTable) where T : ITableEntity, new()
        {
            AsynchronousTableStorageRepository<T> table = abstractTable as AsynchronousTableStorageRepository<T>;
            if (table != null)
            {
                await table.Table.CreateIfNotExistsAsync();
                return;
            }
            throw new ArgumentException($"The table type {abstractTable.GetType().FullName} is not supported for resource management");
        }
    }
}
