using System;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        void Save();
    }
}
