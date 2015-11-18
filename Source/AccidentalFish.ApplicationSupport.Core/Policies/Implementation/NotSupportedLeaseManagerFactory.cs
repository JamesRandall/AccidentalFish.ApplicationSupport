using System;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class NotSupportedLeaseManagerFactory : ILeaseManagerFactory
    {
        public ILeaseManager<T> CreateLeaseManager<T>(string storageAccountConnectionString, string leaseBlockName)
        {
            throw new NotImplementedException();
        }
    }
}