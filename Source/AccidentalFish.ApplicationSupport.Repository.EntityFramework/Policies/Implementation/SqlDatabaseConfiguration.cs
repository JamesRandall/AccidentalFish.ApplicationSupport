using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Runtime.Remoting.Messaging;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies.Implementation
{
    internal class SqlDatabaseConfiguration : DbConfiguration, IDbConfiguration
    {
        public SqlDatabaseConfiguration()
        {
            SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy());
        }

        public IDbExecutionStrategy ExecutionStrategy => new SqlAzureExecutionStrategy();

        public bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    }
}
