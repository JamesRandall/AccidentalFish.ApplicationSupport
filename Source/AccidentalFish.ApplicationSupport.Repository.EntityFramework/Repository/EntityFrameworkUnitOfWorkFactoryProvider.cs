using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal class EntityFrameworkUnitOfWorkFactoryProvider : IUnitOfWorkFactoryProvider
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        public EntityFrameworkUnitOfWorkFactoryProvider(IDbConfiguration dbConfiguration, ILoggerFactory loggerFactory)
        {
            _dbConfiguration = dbConfiguration;
            _loggerFactory = loggerFactory;
        }

        public IUnitOfWorkFactory Create(string contextType, string connectionString)
        {
            return new EntityFrameworkUnitOfWorkFactory(contextType, connectionString, _dbConfiguration, _loggerFactory);
        }
    }
}
