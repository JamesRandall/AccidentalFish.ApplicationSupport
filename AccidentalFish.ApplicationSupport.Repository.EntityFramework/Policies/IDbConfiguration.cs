using System.Data.Entity.Infrastructure;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies
{
    public interface IDbConfiguration
    {
        IDbExecutionStrategy ExecutionStrategy { get; }

        bool SuspendExecutionStrategy { get; set; }
    }
}
