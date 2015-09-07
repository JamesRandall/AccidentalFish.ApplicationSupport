using System;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage
{
    [Serializable]
    public class UnableToAcquireLeaseException : Exception
    {
        public UnableToAcquireLeaseException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
