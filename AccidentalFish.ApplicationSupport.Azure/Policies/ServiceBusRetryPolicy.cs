using System;
using AccidentalFish.ApplicationSupport.Core.Policies;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;

namespace AccidentalFish.ApplicationSupport.Azure.Policies
{
    class ServiceBusRetryPolicy : IRetryPolicy
    {
        private const int MaxRetries = 10;
        private const int Interval = 100;

        private readonly RetryPolicy<ServiceBusTransientErrorDetectionStrategy> _retryPolicy;

        public ServiceBusRetryPolicy()
        {
            _retryPolicy = new RetryPolicy<ServiceBusTransientErrorDetectionStrategy>(MaxRetries, TimeSpan.FromMilliseconds(Interval));
        }

        public void Execute(Action action)
        {
            _retryPolicy.ExecuteAction(action);
        }

        public T Execute<T>(Func<T> func)
        {
            return _retryPolicy.ExecuteAction(func);
        }
    }
}
