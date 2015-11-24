using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;

namespace AccidentalFish.ApplicationSupport.Azure.Policies
{
    internal class LeaseManagerFactory : ILeaseManagerFactory
    {
        private readonly IAzureAssemblyLogger _logger;

        public LeaseManagerFactory(IAzureAssemblyLogger logger)
        {
            _logger = logger;
        }

        public ILeaseManager<T> CreateLeaseManager<T>(string storageAccountConnectionString, string leaseBlockName)
        {
            return new LeaseManager<T>(storageAccountConnectionString, leaseBlockName, _logger);
        }
    }
}
