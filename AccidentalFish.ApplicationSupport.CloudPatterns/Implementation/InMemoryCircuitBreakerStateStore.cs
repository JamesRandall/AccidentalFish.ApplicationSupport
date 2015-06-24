using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.CloudPatterns.Implementation
{
    internal class InMemoryCircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        public DateTime LastChangedStateAt
        {
            get { throw new NotImplementedException(); }
        }

        public CircuitBreakerStateEnum State
        {
            get { throw new NotImplementedException(); }
        }

        public void HalfOpen()
        {
            throw new NotImplementedException();
        }

        public object HalfOpenSyncObject
        {
            get { throw new NotImplementedException(); }
        }

        public Exception LastException
        {
            get { throw new NotImplementedException(); }
        }

        public int IncrementHalfOpenSuccessfulAction()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Trip(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
