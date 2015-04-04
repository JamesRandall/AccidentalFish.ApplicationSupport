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

        IAsynchronousQueue<T> GetQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;

        IAsynchronousTopic<T> GetTopic<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousTopic<T> GetTopic<T>(string topicName, IComponentIdentity componentIdentity) where T : class;
        IAsynchronousSubscription<T> GetSubscription<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousSubscription<T> GetSubscription<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class;
        
        IAsynchronousBlockBlobRepository GetBlockBlobRepository(IComponentIdentity componentIdentity);
        IAsynchronousBlockBlobRepository GetBlockBlobRepository(string containerName, IComponentIdentity componentIdentity);

        string StorageAccountConnectionString(IComponentIdentity componentIdentity);
        string SqlConnectionString(IComponentIdentity componentIdentity);
        string Setting(IComponentIdentity componentIdentity, string settingName);
    }
}
