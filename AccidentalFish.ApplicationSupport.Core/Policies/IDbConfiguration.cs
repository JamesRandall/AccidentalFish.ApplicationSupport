using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    public interface IDbConfiguration
    {
        IDbExecutionStrategy ExecutionStrategy { get; }

        bool SuspendExecutionStrategy { get; set; }
    }
}
