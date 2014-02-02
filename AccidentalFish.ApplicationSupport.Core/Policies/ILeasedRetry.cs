using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    public interface ILeasedRetry
    {
        Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, Func<Task> func);

        Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, bool createLazyLeaseObject, Func<Task> func);

        Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, Func<Task> func);

        Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries, Func<Task> func);

        Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, bool createLazyLeaseObject, Func<Task> func);

        Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries, bool createLazyLeaseObject, Func<Task> func);
    }
}
