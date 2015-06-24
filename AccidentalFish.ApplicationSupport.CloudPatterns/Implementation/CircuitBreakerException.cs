using System;

namespace AccidentalFish.ApplicationSupport.CloudPatterns.Implementation
{
    public class CircuitBreakerException : Exception
    {
        public CircuitBreakerException()
        {
            
        }

        public CircuitBreakerException(string message) : base(message)
        {
            
        }
    }
}
