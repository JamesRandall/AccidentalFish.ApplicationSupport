using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.CloudPatterns
{
    public enum CircuitBreakerStateEnum
    {
        Open,
        HalfOpen,
        Closed
    }

    public interface ICircuitBreaker
    {
        CircuitBreakerStateEnum State { get; }
        Exception LastException { get; }
        DateTime LastStateChange { get; }
        void Close();
        void Open();
        bool IsClosed { get; }
    }
}
