﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Factory for constructing resources using the component configuration system
    /// </summary>
    public interface IAsyncApplicationResourceFactory
    {
        /// <summary>
        /// Create a unit of work factory for the specified component
        /// </summary>
        /// <param name="componentIdentity">Name of the component to create the factory for</param>
        /// <returns>A unit of work factory</returns>
        Task<IUnitOfWorkFactory> GetUnitOfWorkFactoryAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Get a lease manager for the specified component using the default lease block name
        /// </summary>
        /// <typeparam name="T">The type of the key</typeparam>
        /// <param name="componentIdentity">Name of the component to create the lease for - this defines the default lease block name</param>
        /// <returns>A lease manager</returns>
        Task<ILeaseManager<T>> GetLeaseManagerAsync<T>(IComponentIdentity componentIdentity);

        /// <summary>
        /// Get a lease manager for the specified component using the default lease block name
        /// </summary>
        /// <typeparam name="T">The type of the key</typeparam>
        /// <param name="leaseBlockName">The name of the lease block (in Azure this is a blob container)</param>
        /// <param name="componentIdentity">Name of the component to create the lease for - this defines the default lease block name</param>
        /// <returns>A lease manager</returns>
        Task<ILeaseManager<T>> GetLeaseManagerAsync<T>(string leaseBlockName, IComponentIdentity componentIdentity);

        /// <summary>
        /// Get an asynchronous queue implementation for the given component using the default queue name
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <returns>A queue</returns>
        Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Get an asynchronous queue implementation for the given component
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="queuename">The name of the queue</param>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <returns>A queue</returns>
        Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Get an asynchronous queue implementation for the given component using the default queue name
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <param name="queueSerializer">Custom queue serializer</param>
        /// <returns>A queue</returns>
        Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class;

        /// <summary>
        /// Get an asynchronous queue implementation for the given component
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="queuename">The name of the queue</param>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <param name="queueSerializer">Custom queue serializer</param>
        /// <returns>A queue</returns>
        Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class;

        /// <summary>
        /// Get a queue implementation for the given component using the default queue name
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <returns>A queue</returns>
        Task<IQueue<T>> GetQueueAsync<T>(IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Get a queue implementation for the given component
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="queuename">The name of the queue</param>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <returns>A queue</returns>
        Task<IQueue<T>> GetQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Get a queue implementation for the given component using the default queue name
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <param name="queueSerializer">Custom queue serializer</param>
        /// <returns>A queue</returns>
        Task<IQueue<T>> GetQueueAsync<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class;

        /// <summary>
        /// Get a queue implementation for the given component
        /// </summary>
        /// <typeparam name="T">The type of the queue items</typeparam>
        /// <param name="queuename">The name of the queue</param>
        /// <param name="componentIdentity">The name of the component the queue is for</param>
        /// <param name="queueSerializer">Custom queue serializer</param>
        /// <returns>A queue</returns>
        Task<IQueue<T>> GetQueueAsync<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class;

        /// <summary>
        /// Get an asynchronous topic for a component using the default topic name
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="componentIdentity">The name of the component to get the topic for</param>
        /// <returns>A topic</returns>
        Task<IAsynchronousTopic<T>> GetAsyncTopicAsync<T>(IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Get an asynchronous topic for a component
        /// </summary>
        /// <typeparam name="T">The type of the topic items</typeparam>
        /// <param name="topicName">The name of the topic</param>
        /// <param name="componentIdentity">The name of the component to get the topic for</param>
        /// <returns>A topic</returns>
        Task<IAsynchronousTopic<T>> GetAsyncTopicAsync<T>(string topicName, IComponentIdentity componentIdentity) where T : class;
        /// <summary>
        /// Get an asynchronous subscription to a topic for the default topic and subscription
        /// </summary>
        /// <typeparam name="T">The type of the subscription / topic items</typeparam>
        /// <param name="componentIdentity">The name of the component</param>
        /// <returns>A subscription</returns>
        Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(IComponentIdentity componentIdentity) where T : class;
        /// <summary>
        /// Get an asynchronous subscription to a topic for the default topic and named subscription
        /// </summary>
        /// <typeparam name="T">The type of the subscription / topic items</typeparam>
        /// <param name="subscriptionName">The name of the subscription</param>
        /// <param name="componentIdentity">The name of the component</param>
        /// <returns>A subscription</returns>
        Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Get an asynchronous subscription to a topic for the named topic and subscription
        /// </summary>
        /// <typeparam name="T">The type of the subscription / topic items</typeparam>
        /// <param name="topicName">The name of the topic</param>
        /// <param name="componentIdentity">The name of the component</param>
        /// <param name="subscriptionName">The name of the subscription</param>
        /// <returns>A subscription</returns>
        Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(string subscriptionName, string topicName, IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Get the default block blob repository for the component
        /// </summary>
        /// <param name="componentIdentity">The name of the component</param>
        /// <returns>A blob repository</returns>
        Task<IAsynchronousBlockBlobRepository> GetAsyncBlockBlobRepositoryAsync(IComponentIdentity componentIdentity);
        /// <summary>
        /// Get a block repository for the component for the given container
        /// </summary>
        /// <param name="containerName">The name of the container</param>
        /// <param name="componentIdentity">The name of the component</param>
        /// <returns>A blob repositry</returns>
        Task<IAsynchronousBlockBlobRepository> GetAsyncBlockBlobRepositoryAsync(string containerName, IComponentIdentity componentIdentity);

        /// <summary>
        /// Get the storage account connection string for the component
        /// </summary>
        /// <param name="componentIdentity">The name of the component</param>
        /// <returns>A storage account connection string</returns>
        Task<string> StorageAccountConnectionStringAsync(IComponentIdentity componentIdentity);
        
        /// <summary>
        /// Get the SQL connection string for the component
        /// </summary>
        /// <param name="componentIdentity">The name of the component</param>
        /// <returns>A SQL connection string</returns>
        Task<string> SqlConnectionStringAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Get an application setting for the component
        /// </summary>
        /// <param name="componentIdentity">The name of the component</param>
        /// <param name="settingName">The name of the setting</param>
        /// <returns>The value for the setting</returns>
        Task<string> SettingAsync(IComponentIdentity componentIdentity, string settingName);
    }
}
