using System;
using System.Collections.Generic;
using System.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    class FactoryBackoffPolicyTimingProvider : IBackoffPolicyTimingProvider
    {
        private readonly IReadOnlyCollection<TimeSpan> _values;

        public FactoryBackoffPolicyTimingProvider(IReadOnlyCollection<TimeSpan> backoffTimings)
        {
            _values = backoffTimings;
        }

        public IReadOnlyCollection<TimeSpan> GetIntervals()
        {
            return _values;
        }
    }
}
