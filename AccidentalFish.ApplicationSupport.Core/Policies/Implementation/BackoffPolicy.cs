using System;
using System.Collections.Generic;
using System.Threading;
using AccidentalFish.ApplicationSupport.Core.Threading;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class BackoffPolicy : IBackoffPolicy
    {
        private static readonly List<int> BackoffTimings = new List<int> { 100, 250, 500, 1000, 5000 };

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
                
                // TODO: Refactor to get rid of wait handle
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
