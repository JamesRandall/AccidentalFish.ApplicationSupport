using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Extensions;
using AccidentalFish.ApplicationSupport.Core.Logging;
using static System.String;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class LeasedRetry : ILeasedRetry
    {
        private readonly ILogger _logger;

        public LeasedRetry(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.GetAssemblyLogger();
        }

        public Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, Func<Task> func)
        {
            return RetryAsync(leaseManager, key, TimeSpan.FromSeconds(30), 10, false, func);
        }

        public Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, Func<Task> func)
        {
            return RetryAsync(leaseManager, key, leaseDuration, 30, false, func);
        }

        public Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, bool createLazyLeaseObject, Func<Task> func)
        {
            return RetryAsync(leaseManager, key, TimeSpan.FromSeconds(30), 10, createLazyLeaseObject, func);
        }

        public Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries, Func<Task> func)
        {
            return RetryAsync(leaseManager, key, leaseDuration, maxRetries, false, func);
        }

        public Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, bool createLazyLeaseObject, Func<Task> func)
        {
            return RetryAsync(leaseManager, key, leaseDuration, 10, createLazyLeaseObject, func);
        }

        public async Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries,
            bool createLazyLeaseObject, Func<Task> func)
        {
            TimeSpan retryDelay = TimeSpan.FromSeconds(leaseDuration.TotalSeconds / maxRetries);
            string leaseId = null;
            int retry = 0;

            while (IsNullOrWhiteSpace(leaseId) && retry < maxRetries)
            {
                _logger?.Verbose("LeasedRetry - RetryAsync - attempting to acquire lease {0}, retry {1}", key, retry);
                try
                {
                    leaseId = await leaseManager.LeaseAsync(key, leaseDuration);
                }
                catch (Exception)
                {
                    leaseId = null;
                }

                if (IsNullOrWhiteSpace(leaseId))
                {
                    if (retry == 0)
                    {
                        await leaseManager.CreateLeaseObjectIfNotExistAsync(key);
                    }
                    retry++;
                    await Task.Delay(retryDelay);
                }
            }

            if (IsNullOrWhiteSpace(leaseId))
            {
                _logger?.Verbose("LeasedRetry - RetryAsync - failed to acquire lease {0} after retry {1} and giving up", key, retry);
                return false;
            }

            _logger?.Verbose("LeasedRetry - RetryAsync - acquired lease {0} after retry {1}", key, retry);

            try
            {
                await func();
            }
            finally
            {
                leaseManager.ReleaseAsync(key, leaseId).Wait();
                _logger?.Verbose("LeasedRetry - RetryAsync - released lease {0}", key);
            }

            return true;
        }
    }
}
