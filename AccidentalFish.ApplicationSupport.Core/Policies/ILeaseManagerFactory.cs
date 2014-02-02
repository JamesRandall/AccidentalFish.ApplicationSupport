using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    public interface ILeaseManagerFactory
    {
        ILeaseManager<T> CreateLeaseManager<T>(string storageAccountConnectionString, string leaseBlockName);
    }
}
