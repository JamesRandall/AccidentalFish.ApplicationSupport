using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.CloudPatterns.Implementation
{
    internal interface ICircuitBreakerStateStore
    {
        DateTime LastChangedStateAt { get; }
        CircuitBreakerStateEnum State { get; }
        void HalfOpen();
        object HalfOpenSyncObject { get; }
        Exception LastException { get; }
        int IncrementHalfOpenSuccessfulAction();
        void Reset();
        void Trip(Exception exception);
    }
}
