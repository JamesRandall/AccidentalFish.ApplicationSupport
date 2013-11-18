using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AccidentalFish.ApplicationSupport.Core.Policies;

namespace AccidentalFish.ApplicationSupport.Azure.Policies
{
    internal class NullDatabaseConfiguration : DbConfiguration, IDbConfiguration
    {
        public NullDatabaseConfiguration()
        {
            
        }

        public IDbExecutionStrategy ExecutionStrategy { get { return new DefaultExecutionStrategy();} }
        public bool SuspendExecutionStrategy { get; set; }
    }
}
