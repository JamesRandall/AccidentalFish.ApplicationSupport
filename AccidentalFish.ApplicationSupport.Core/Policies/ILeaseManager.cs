using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    public interface ILeaseManager<in T>
    {
        Task<bool> CreateLeaseObjectIfNotExist(T key);
        Task<string> Lease(T key);
        Task<string> Lease(T key, TimeSpan leaseTime);
        Task Release(T key, string leaseId);
        Task Renew(T key, string leaseId);
    }
}
