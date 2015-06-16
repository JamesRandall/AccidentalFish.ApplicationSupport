using System.Data.Entity;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Injection;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies.Implementation;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework
{
    public static class Bootstrapper
    {
        public enum SqlDatabaseTypeEnum
        {
            Azure,
            Lan
        }

        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            RegisterDependencies(dependencyResolver, SqlDatabaseTypeEnum.Azure);
        }

        public static void RegisterDependencies(IDependencyResolver dependencyResolver, SqlDatabaseTypeEnum databaseType)
        {
            dependencyResolver.Register<IUnitOfWorkFactory, EntityFrameworkUnitOfWorkFactory>();

            if (databaseType == SqlDatabaseTypeEnum.Azure)
            {
                dependencyResolver.Register<IDbConfiguration, SqlDatabaseConfiguration>();
                DbConfiguration.SetConfiguration(new SqlDatabaseConfiguration());
            }
            else
            {
                dependencyResolver.Register<IDbConfiguration, NullDatabaseConfiguration>();
                DbConfiguration.SetConfiguration(new NullDatabaseConfiguration());
            }

            dependencyResolver.Register<IUnitOfWorkFactoryProvider, EntityFrameworkUnitOfWorkFactoryProvider>();
        }
    }
}
