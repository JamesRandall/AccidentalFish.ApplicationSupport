using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    public interface ILeaseManager<in T>
    {
        Task<bool> CreateLeaseObjectIfNotExist(T key);
        Task<string> Lease(T key);
        Task<string> Lease(T key, TimeSpan leaseTime);
        Task Release(T key, string leaseId);
    }
}
