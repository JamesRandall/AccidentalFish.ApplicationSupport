using System;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    public interface IEntityFrameworkUnitOfWork
    {
        void Execute(Action action);
        T2 Execute<T2>(Func<T2> action);
        bool SuspendExecutionPolicy { get; set; }
    }
}
