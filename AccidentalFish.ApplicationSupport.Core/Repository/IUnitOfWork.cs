using System;
using AccidentalFish.ApplicationSupport.Core.Policies;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        void Save();
        void Execute(Action action);
        T2 Execute<T2>(Func<T2> action);
        bool SuspendExecutionPolicy { get; set; }

    }
}
