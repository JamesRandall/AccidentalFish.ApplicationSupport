using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class BackoffPolicyDefaultTimingProvider : IBackoffPolicyTimingProvider
    {
        public IReadOnlyCollection<TimeSpan> GetIntervals()
        {
            return new ReadOnlyCollection<TimeSpan>(new[]
            {
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromMilliseconds(1000),
                TimeSpan.FromMilliseconds(5000)
            });
        }
    }
}
