using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.CloudPatterns.Implementation
{
    internal class CircuitBreaker : ICircuitBreaker
    {
        private readonly ICircuitBreakerStateStore _stateStore;

        public CircuitBreaker(ICircuitBreakerStateStore stateStore)
        {
            _stateStore = stateStore;
        }

        public CircuitBreakerStateEnum State
        {
            get { throw new NotImplementedException(); }
        }

        public Exception LastException
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastStateChange
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }
    }
}
