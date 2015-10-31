using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Blobs;
using AccidentalFish.ApplicationSupport.Azure.Queues;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Implementation
{
    internal class AzureResourceManager : IAzureResourceManager
    {
        public async Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousQueue<T> abstractQueue) where T : class
        {
            AsynchronousQueue<T> storageQueue = abstractQueue as AsynchronousQueue<T>;
            if (storageQueue != null)
            {
                return (await storageQueue.UnderlyingQueue.CreateIfNotExistsAsync());
            }
            AsynchronousBrokeredMessageQueue<T> serviceBusQueue = abstractQueue as AsynchronousBrokeredMessageQueue<T>;
            if (serviceBusQueue != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusQueue.ConnectionString);
                if (!(await namespaceManager.QueueExistsAsync(serviceBusQueue.UnderlyingQueue.Path)))
                {
                    await namespaceManager.CreateQueueAsync(serviceBusQueue.UnderlyingQueue.Path);
                    return true;
                }
                return false;
            }

            throw new ArgumentException($"The queue type {abstractQueue.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> DeleteIfExistsAsync<T>(IAsynchronousQueue<T> abstractQueue) where T : class
        {
            AsynchronousQueue<T> storageQueue = abstractQueue as AsynchronousQueue<T>;
            if (storageQueue != null)
            {
                return (await storageQueue.UnderlyingQueue.DeleteIfExistsAsync());
            }
            AsynchronousBrokeredMessageQueue<T> serviceBusQueue = abstractQueue as AsynchronousBrokeredMessageQueue<T>;
            if (serviceBusQueue != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusQueue.ConnectionString);
                if ((await namespaceManager.QueueExistsAsync(serviceBusQueue.UnderlyingQueue.Path)))
                {
                    await namespaceManager.DeleteQueueAsync(serviceBusQueue.UnderlyingQueue.Path);
                    return true;
                }
                return false;
            }

            throw new ArgumentException($"The queue type {abstractQueue.GetType().FullName} is not supported for resource management");
        }

        public bool CreateIfNotExists<T>(IQueue<T> abstractQueue) where T : class
        {
            StorageQueue<T> storageQueue = abstractQueue as StorageQueue<T>;
            if (storageQueue != null)
            {
                return storageQueue.UnderlyingQueue.CreateIfNotExists();
            }
            BrokeredMessageQueue<T> serviceBusQueue = abstractQueue as BrokeredMessageQueue<T>;
            if (serviceBusQueue != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusQueue.ConnectionString);
                if (!namespaceManager.QueueExists(serviceBusQueue.UnderlyingQueue.Path))
                {
                    namespaceManager.CreateQueue(serviceBusQueue.UnderlyingQueue.Path);
                    return true;
                }
                return false;
            }
            throw new ArgumentException($"The queue type {abstractQueue.GetType().FullName} is not supported for resource management");
        }

        public bool DeleteIfExists<T>(IQueue<T> abstractQueue) where T : class
        {
            StorageQueue<T> storageQueue = abstractQueue as StorageQueue<T>;
            if (storageQueue != null)
            {
                return storageQueue.UnderlyingQueue.DeleteIfExists();
            }
            BrokeredMessageQueue<T> serviceBusQueue = abstractQueue as BrokeredMessageQueue<T>;
            if (serviceBusQueue != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusQueue.ConnectionString);
                if (namespaceManager.QueueExists(serviceBusQueue.UnderlyingQueue.Path))
                {
                    namespaceManager.DeleteQueue(serviceBusQueue.UnderlyingQueue.Path);
                    return true;
                }
                return false;
            }
            throw new ArgumentException($"The queue type {abstractQueue.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousTopic<T> abstractTopic) where T : class
        {
            AsynchronousTopic<T> topic = abstractTopic as AsynchronousTopic<T>;
            if (topic != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(topic.ConnectionString);
                if (!(await namespaceManager.QueueExistsAsync(topic.UnderlyingTopic.Path)))
                {
                    await namespaceManager.CreateTopicAsync(topic.UnderlyingTopic.Path);
                    return true;
                }
                return false;
            }
            throw new ArgumentException($"The topic type {abstractTopic.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> DeleteIfExistsAsync<T>(IAsynchronousTopic<T> abstractTopic) where T : class
        {
            AsynchronousTopic<T> topic = abstractTopic as AsynchronousTopic<T>;
            if (topic != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(topic.ConnectionString);
                if ((await namespaceManager.QueueExistsAsync(topic.UnderlyingTopic.Path)))
                {
                    await namespaceManager.DeleteTopicAsync(topic.UnderlyingTopic.Path);
                    return true;
                }
                return false;
            }
            throw new ArgumentException($"The topic type {abstractTopic.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousSubscription<T> abstractSubscription) where T : class
        {
            AsynchronousSubscription<T> subscription = abstractSubscription as AsynchronousSubscription<T>;
            if (subscription != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(subscription.ConnectionString);
                if (!(await namespaceManager.QueueExistsAsync(subscription.UnderlyingSubscription.Name)))
                {
                    await namespaceManager.CreateSubscriptionAsync(subscription.UnderlyingSubscription.TopicPath, subscription.UnderlyingSubscription.Name);
                    return true;
                }
                return false;
            }
            throw new ArgumentException($"The topic type {abstractSubscription.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousSubscription<T> abstractSubscription, Filter filter) where T : class
        {
            AsynchronousSubscription<T> subscription = abstractSubscription as AsynchronousSubscription<T>;
            if (subscription != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(subscription.ConnectionString);
                if (!(await namespaceManager.QueueExistsAsync(subscription.UnderlyingSubscription.Name)))
                {
                    await namespaceManager.CreateSubscriptionAsync(subscription.UnderlyingSubscription.TopicPath, subscription.UnderlyingSubscription.Name, filter);
                    return true;
                }
                return false;
            }
            throw new ArgumentException($"The topic type {abstractSubscription.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> DeleteIfExistsAsync<T>(IAsynchronousSubscription<T> abstractSubscription) where T : class
        {
            AsynchronousSubscription<T> subscription = abstractSubscription as AsynchronousSubscription<T>;
            if (subscription != null)
            {
                NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(subscription.ConnectionString);
                if ((await namespaceManager.QueueExistsAsync(subscription.UnderlyingSubscription.Name)))
                {
                    await namespaceManager.DeleteSubscriptionAsync(subscription.UnderlyingSubscription.TopicPath, subscription.UnderlyingSubscription.Name);
                    return true;
                }
                return false;
            }
            throw new ArgumentException($"The topic type {abstractSubscription.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousTableStorageRepository<T> abstractTable) where T : ITableEntity, new()
        {
            AsynchronousTableStorageRepository<T> table = abstractTable as AsynchronousTableStorageRepository<T>;
            if (table != null)
            {
                return await table.Table.CreateIfNotExistsAsync();
            }
            throw new ArgumentException($"The table type {abstractTable.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> DeleteIfExistsAsync<T>(IAsynchronousTableStorageRepository<T> abstractTable) where T : ITableEntity, new()
        {
            AsynchronousTableStorageRepository<T> table = abstractTable as AsynchronousTableStorageRepository<T>;
            if (table != null)
            {
                return (await table.Table.DeleteIfExistsAsync());
            }
            throw new ArgumentException($"The table type {abstractTable.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> CreateIfNotExistsAsync(IAsynchronousBlockBlobRepository abstractRepository)
        {
            AsynchronousBlockBlobRepository repository = abstractRepository as AsynchronousBlockBlobRepository;
            if (repository != null)
            {
                return await repository.Container.CreateIfNotExistsAsync();
            }
            throw new ArgumentException($"The blob container type {abstractRepository.GetType().FullName} is not supported for resource management");
        }

        public async Task<bool> DeleteIfExistsAsync(IAsynchronousBlockBlobRepository abstractRepository)
        {
            AsynchronousBlockBlobRepository repository = abstractRepository as AsynchronousBlockBlobRepository;
            if (repository != null)
            {
                return await repository.Container.DeleteIfExistsAsync();
            }
            throw new ArgumentException($"The blob container type {abstractRepository.GetType().FullName} is not supported for resource management");
        }
    }
}
