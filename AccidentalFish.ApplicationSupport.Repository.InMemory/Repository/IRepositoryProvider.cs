using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Repository
{
    internal interface IRepositoryProvider
    {
        IRepositoryAsync<T> GetAsyncRepository<T>(List<T> unitOfWorkChanges) where T : class;
    }
}