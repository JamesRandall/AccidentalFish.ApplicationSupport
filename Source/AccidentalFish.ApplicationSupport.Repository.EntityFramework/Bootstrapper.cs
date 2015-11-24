using System;
using System.Data.Entity;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Logging;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Logging.Implementation;
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

        public static IDependencyResolver UseEntityFramework(this IDependencyResolver dependencyResolver)
        {
            return UseEntityFramework(dependencyResolver, SqlDatabaseTypeEnum.Azure);
        }

        public static IDependencyResolver UseEntityFramework(this IDependencyResolver dependencyResolver, SqlDatabaseTypeEnum databaseType)
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
            // internal
            dependencyResolver.Register<IEntityFrameworkRepositoryLogger>(() =>
                new EntityFrameworkRepositoryLogger(
                    dependencyResolver.Resolve<ILoggerFactory>()
                        .CreateLogger(new LoggerSource("AccidentalFish.ApplicationSupport.Repository.EntityFramework"))));
            return dependencyResolver;
        }
    }
}
