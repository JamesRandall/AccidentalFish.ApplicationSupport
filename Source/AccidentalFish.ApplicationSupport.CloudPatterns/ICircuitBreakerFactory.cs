using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.CloudPatterns
{
    public interface ICircuitBreakerFactory
    {
        /// <summary>
        /// Creates a circuit break with the specified fully qualified name.
        /// All operations in a circuit breaker with the same FQN share circuit breaker state
        /// </summary>
        /// <param name="fullyQualifiedName"></param>
        /// <returns></returns>
        ICircuitBreaker Create(IFullyQualifiedName fullyQualifiedName);
    }
}
