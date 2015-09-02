using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class LeasedRetry : ILeasedRetry
    {
        public Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, Func<Task> func)
        {
            return Retry(leaseManager, key, TimeSpan.FromSeconds(30), 10, false, func);
        }

        public Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, Func<Task> func)
        {
            return Retry(leaseManager, key, leaseDuration, 30, false, func);
        }

        public Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, bool createLazyLeaseObject, Func<Task> func)
        {
            return Retry(leaseManager, key, TimeSpan.FromSeconds(30), 10, createLazyLeaseObject, func);
        }

        public Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries, Func<Task> func)
        {
            return Retry(leaseManager, key, leaseDuration, maxRetries, false, func);
        }

        public Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, bool createLazyLeaseObject, Func<Task> func)
        {
            return Retry(leaseManager, key, leaseDuration, 10, createLazyLeaseObject, func);
        }

        public async Task<bool> Retry<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries,
            bool createLazyLeaseObject, Func<Task> func)
        {
            TimeSpan retryDelay = TimeSpan.FromSeconds(leaseDuration.TotalSeconds / maxRetries);
            string leaseId = null;
            int retry = 0;

            while (String.IsNullOrWhiteSpace(leaseId) && retry < maxRetries)
            {
                try
                {
                    leaseId = await leaseManager.Lease(key, leaseDuration);
                }
                catch (Exception)
                {
                    leaseId = null;
                }

                if (String.IsNullOrWhiteSpace(leaseId))
                {
                    if (retry == 0)
                    {
                        await leaseManager.CreateLeaseObjectIfNotExist(key);
                    }
                    retry++;
                    await Task.Delay(retryDelay);
                }
            }

            if (String.IsNullOrWhiteSpace(leaseId)) return false;

            try
            {
                await func();
            }
            finally
            {
                leaseManager.Release(key, leaseId).Wait();
            }

            return true;
        }
    }
}
