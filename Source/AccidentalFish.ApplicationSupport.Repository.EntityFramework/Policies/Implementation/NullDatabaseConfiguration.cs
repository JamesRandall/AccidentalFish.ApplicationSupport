using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies.Implementation
{
    internal class NullDatabaseConfiguration : DbConfiguration, IDbConfiguration
    {
        public IDbExecutionStrategy ExecutionStrategy => new DefaultExecutionStrategy();
        public bool SuspendExecutionStrategy { get; set; }
    }
}
