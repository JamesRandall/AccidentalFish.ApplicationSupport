using System;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Retry policies are used to wrap tasks where temporary failure (transient faults) are expected.
    /// </summary>
    public interface IRetryPolicy
    {
        void Execute(Action action);
        T Execute<T>(Func<T> func);
    }
}
