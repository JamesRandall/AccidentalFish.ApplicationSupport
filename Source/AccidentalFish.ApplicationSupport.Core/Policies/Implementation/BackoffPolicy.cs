using System;
using System.Collections.Generic;
using System.Threading;
using AccidentalFish.ApplicationSupport.Core.Extensions;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Threading;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class BackoffPolicy : IBackoffPolicy
    {
        private static readonly List<int> BackoffTimings = new List<int> { 100, 250, 500, 1000, 5000 };
        private readonly ICoreAssemblyLogger _logger;

        public BackoffPolicy(ICoreAssemblyLogger logger)
        {
            _logger = logger;
        }

        public void Execute(Func<bool> function, IWaitHandle waitHandle)
        {
            do
            {
                bool result = true;
                while (result && !waitHandle.IsSet)
                {
                    result = function();
                }

                IEnumerator<int> enumerator = BackoffTimings.GetEnumerator();
                enumerator.MoveNext();
                int currentWaitTime = enumerator.Current;
                if (!result)
                {
                    _logger?.Verbose("BackoffPolicy - backing off to {0}ms", currentWaitTime);
                }
                while (!result && !waitHandle.Wait(currentWaitTime))
                {
                    result = function();
                    if (!result)
                    {
                        if (enumerator.MoveNext())
                        {
                            currentWaitTime = enumerator.Current;
                        }
                    }
                }
            }
            while (!waitHandle.IsSet);
        }

        public void Execute(Func<bool> function, CancellationToken cancellationToken)
        {
            do
            {
                bool result = true;
                while (result && !cancellationToken.IsCancellationRequested)
                {
                    result = function();
                }

                IEnumerator<int> enumerator = BackoffTimings.GetEnumerator();
                enumerator.MoveNext();
                int currentWaitTime = enumerator.Current;

                if (!result)
                {
                    _logger?.Verbose("BackoffPolicy - backing off to {0}ms", currentWaitTime);
                }
                while (!result && !cancellationToken.WaitHandle.WaitOne(currentWaitTime))
                {
                    result = function();
                    if (!result)
                    {
                        if (enumerator.MoveNext())
                        {
                            currentWaitTime = enumerator.Current;
                        }
                    }
                }
            }
            while (!cancellationToken.IsCancellationRequested);
        }
    }
}
