using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal class EntityFrameworkUnitOfWorkFactoryProvider : IUnitOfWorkFactoryProvider
    {
        private readonly IDbConfiguration _dbConfiguration;

        public EntityFrameworkUnitOfWorkFactoryProvider(IDbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public IUnitOfWorkFactory Create(string contextType, string connectionString)
        {
            return new EntityFrameworkUnitOfWorkFactory(contextType, connectionString, _dbConfiguration);
        }
    }
}
