using AccidentalFish.ApplicationSupport.Azure.NoSql;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Policies;

namespace AccidentalFish.ApplicationSupport.Azure.Policies
{
    internal class LeaseManagerFactory : ILeaseManagerFactory
    {
        public ILeaseManager<T> CreateLeaseManager<T>(string storageAccountConnectionString, string leaseBlockName)
        {
            return new LeaseManager<T>(storageAccountConnectionString, leaseBlockName);
        }
    }
}
