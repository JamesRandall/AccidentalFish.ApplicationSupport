using System;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Logging;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    class EntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory _contextFactory;
        private readonly IDbConfiguration _dbConfiguration;
        private readonly Type _contextType;
        private readonly IEntityFrameworkRepositoryLogger _logger;

        private EntityFrameworkUnitOfWorkFactory(IEntityFrameworkRepositoryLogger logger)
        {
            _logger = logger;
        }

        public EntityFrameworkUnitOfWorkFactory(
            IConfiguration configuration,
            IDbContextFactory contextFactory,
            IDbConfiguration dbConfiguration,
            IEntityFrameworkRepositoryLogger logger) : this(logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (contextFactory == null) throw new ArgumentNullException(nameof(contextFactory));
            if (dbConfiguration == null) throw new ArgumentNullException(nameof(dbConfiguration));

            _configuration = configuration;
            _contextFactory = contextFactory;
            _dbConfiguration = dbConfiguration;
            _contextType = null;
        }

        public EntityFrameworkUnitOfWorkFactory(
            string contextType,
            string connectionString,
            IDbConfiguration dbConfiguration,
            IEntityFrameworkRepositoryLogger logger) : this(logger)
        {
            if (String.IsNullOrWhiteSpace(contextType)) throw new ArgumentNullException(nameof(contextType));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (dbConfiguration == null) throw new ArgumentNullException(nameof(dbConfiguration));

            _contextType = Type.GetType(contextType);
            _connectionString = connectionString;
            _dbConfiguration = dbConfiguration;

            if (_contextType == null)
            {
                // deliberately not logged as an error as this throws an exception - it is for the callee to decide
                // if this is an error
                _logger?.Verbose("EntityFrameworkUnitOfWorkFactory: Unable to locate context type {0}", _contextType);
                throw new InvalidOperationException($"Unable to locate context type {_contextType}");
            }
        }

        public IUnitOfWork Create()
        {
            return _contextType == null ?
                new EntityFrameworkUnitOfWork(_configuration, _contextFactory, _dbConfiguration, _logger) :
                new EntityFrameworkUnitOfWork(_contextType, _connectionString, _dbConfiguration, _logger);
        }

        public IUnitOfWorkAsync CreateAsync()
        {
            return _contextType == null ?
                new EntityFrameworkUnitOfWorkAsync(_configuration, _contextFactory, _dbConfiguration, _logger) :
                new EntityFrameworkUnitOfWorkAsync(_contextType, _connectionString, _dbConfiguration, _logger);
        }
    }
}
