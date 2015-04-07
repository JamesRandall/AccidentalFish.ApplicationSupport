using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public interface IApplicationResourceFactory
    {
        IUnitOfWorkFactory GetUnitOfWorkFactory(IComponentIdentity componentIdentity);

        ILeaseManager<T> GetLeaseManager<T>(IComponentIdentity componentIdentity);
        ILeaseManager<T> GetLeaseManager<T>(string leaseBlockName, IComponentIdentity componentIdentity);

        IAsynchronousQueue<T> GetAsyncQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetAsyncQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetAsyncBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetAsyncBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
        
        IQueue<T> GetQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
        IQueue<T> GetBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IQueue<T> GetBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;

        IAsynchronousTopic<T> GetAsyncTopic<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousTopic<T> GetAsyncTopic<T>(string topicName, IComponentIdentity componentIdentity) where T : class;
        IAsynchronousSubscription<T> GetAsyncSubscription<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousSubscription<T> GetAsyncSubscription<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class;
        
        IAsynchronousBlockBlobRepository GetAsyncBlockBlobRepository(IComponentIdentity componentIdentity);
        IAsynchronousBlockBlobRepository GetAsyncBlockBlobRepository(string containerName, IComponentIdentity componentIdentity);

        string StorageAccountConnectionString(IComponentIdentity componentIdentity);
        string SqlConnectionString(IComponentIdentity componentIdentity);
        string Setting(IComponentIdentity componentIdentity, string settingName);
    }
}
