using System;
using System.Runtime.Remoting.Contexts;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Policies;
using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Core.Repository.Implementaton
{
    class EntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory _contextFactory;
        private readonly IDbConfiguration _dbConfiguration;
        private readonly Type _contextType;

        public EntityFrameworkUnitOfWorkFactory(IConfiguration configuration, IDbContextFactory contextFactory, IDbConfiguration dbConfiguration)
        {
            Condition.Requires(configuration).IsNotNull();
            Condition.Requires(contextFactory).IsNotNull();
            Condition.Requires(dbConfiguration).IsNotNull();

            _configuration = configuration;
            _contextFactory = contextFactory;
            _dbConfiguration = dbConfiguration;
            _contextType = null;
        }

        public EntityFrameworkUnitOfWorkFactory(
            string contextType,
            string connectionString,
            IDbConfiguration dbConfiguration)
        {
            Condition.Requires(contextType).IsNotNullOrWhiteSpace();
            Condition.Requires(connectionString).IsNotNullOrWhiteSpace();
            Condition.Requires(dbConfiguration).IsNotNull();

            _contextType = Type.GetType(contextType);
            _connectionString = connectionString;
            _dbConfiguration = dbConfiguration;

            Condition.Ensures(_contextType).IsNotNull();
        }

        public IUnitOfWork Create()
        {
            return _contextType == null ?
                new EntityFrameworkUnitOfWork(_configuration, _contextFactory, _dbConfiguration) :
                new EntityFrameworkUnitOfWork(_contextType, _connectionString, _dbConfiguration);
        }

        public IUnitOfWorkAsync CreateAsync()
        {
            return _contextType == null ?
                new EntityFrameworkUnitOfWorkAsync(_configuration, _contextFactory, _dbConfiguration) :
                new EntityFrameworkUnitOfWorkAsync(_contextType, _connectionString, _dbConfiguration);
        }
    }
}
