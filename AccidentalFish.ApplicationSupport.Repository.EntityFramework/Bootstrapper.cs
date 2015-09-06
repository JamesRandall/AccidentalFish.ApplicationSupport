﻿using System;
using System.Data.Entity;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.DependencyResolver;
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
            return dependencyResolver;
        }

        [Obsolete]
        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            UseEntityFramework(dependencyResolver);
        }

        [Obsolete]
        public static void RegisterDependencies(IDependencyResolver dependencyResolver, SqlDatabaseTypeEnum databaseType)
        {
            UseEntityFramework(dependencyResolver, databaseType);
        }
    }
}
