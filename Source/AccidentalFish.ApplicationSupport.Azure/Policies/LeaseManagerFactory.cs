using AccidentalFish.ApplicationSupport.Azure.Extensions;
using AccidentalFish.ApplicationSupport.Azure.NoSql;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;

namespace AccidentalFish.ApplicationSupport.Azure.Policies
{
    internal class LeaseManagerFactory : ILeaseManagerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public LeaseManagerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ILeaseManager<T> CreateLeaseManager<T>(string storageAccountConnectionString, string leaseBlockName)
        {
            return new LeaseManager<T>(storageAccountConnectionString, leaseBlockName, _loggerFactory.GetAssemblyLogger());
        }
    }
}
