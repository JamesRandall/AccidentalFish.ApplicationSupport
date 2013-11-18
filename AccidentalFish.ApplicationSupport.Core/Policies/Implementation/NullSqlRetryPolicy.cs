using System;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    class NullSqlRetryPolicy : ISqlRetryPolicy
    {
        public void Execute(Action action)
        {
            action();
        }

        public T Execute<T>(Func<T> func)
        {
            return func();
        }
    }
}
