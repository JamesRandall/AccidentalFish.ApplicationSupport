using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
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
    }
}
