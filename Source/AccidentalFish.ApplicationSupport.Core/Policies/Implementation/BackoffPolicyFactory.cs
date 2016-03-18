using System;
using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class BackoffPolicyFactory : IBackoffPolicyFactory
    {
        private readonly ICoreAssemblyLogger _coreAssemblyLogger;

        public BackoffPolicyFactory(ICoreAssemblyLogger coreAssemblyLogger)
        {
            _coreAssemblyLogger = coreAssemblyLogger;
        }

        public IAsynchronousBackoffPolicy CreateAsynchronousBackoffPolicy(IReadOnlyCollection<TimeSpan> backoffTimings = null)
        {
            return new AsynchronousBackoffPolicy(_coreAssemblyLogger, new FactoryBackoffPolicyTimingProvider(backoffTimings));
        }

        public IBackoffPolicy CreateBackoffPolicy(IReadOnlyCollection<TimeSpan> backoffTimings = null)
        {
            return new BackoffPolicy(_coreAssemblyLogger, new FactoryBackoffPolicyTimingProvider(backoffTimings));
        }
    }
}
