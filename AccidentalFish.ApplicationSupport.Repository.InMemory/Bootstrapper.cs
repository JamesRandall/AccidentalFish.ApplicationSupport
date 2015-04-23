using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Repository.InMemory.Helpers;
using AccidentalFish.ApplicationSupport.Repository.InMemory.Helpers.Implementation;
using AccidentalFish.ApplicationSupport.Repository.InMemory.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterInstance<IEntityCopy>(new EntityCopy(new CopyCompiler()));
            dependencyResolver.RegisterInstance<IRepositoryProvider>(new RepositoryProvider());
        }
    }
}
