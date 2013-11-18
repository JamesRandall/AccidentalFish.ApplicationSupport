using System;
using AccidentalFish.ApplicationSupport.Core.Policies;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
using Microsoft.Practices.TransientFaultHandling;

namespace AccidentalFish.ApplicationSupport.Azure.Policies
{
    internal class SqlAzureRetryPolicy : ISqlRetryPolicy
    {
        private const int MaxRetries = 10;
        private const int Interval = 100;
        private readonly RetryPolicy<SqlAzureTransientErrorDetectionStrategy> _retryPolicy;
        
        public SqlAzureRetryPolicy()
        {
            _retryPolicy = new RetryPolicy<SqlAzureTransientErrorDetectionStrategy>(MaxRetries, TimeSpan.FromMilliseconds(Interval));
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
