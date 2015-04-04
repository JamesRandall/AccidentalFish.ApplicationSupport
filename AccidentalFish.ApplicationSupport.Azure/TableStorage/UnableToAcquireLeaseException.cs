using System;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage
{
    public class UnableToAcquireLeaseException : Exception
    {
        public UnableToAcquireLeaseException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
