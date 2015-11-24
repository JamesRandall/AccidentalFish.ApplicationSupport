using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Logging;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal class EntityFrameworkUnitOfWorkFactoryProvider : IUnitOfWorkFactoryProvider
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly IEntityFrameworkRepositoryLogger _logger;

        public EntityFrameworkUnitOfWorkFactoryProvider(IDbConfiguration dbConfiguration, IEntityFrameworkRepositoryLogger logger)
        {
            _dbConfiguration = dbConfiguration;
            _logger = logger;
        }

        public IUnitOfWorkFactory Create(string contextType, string connectionString)
        {
            return new EntityFrameworkUnitOfWorkFactory(contextType, connectionString, _dbConfiguration, _logger);
        }
    }
}
