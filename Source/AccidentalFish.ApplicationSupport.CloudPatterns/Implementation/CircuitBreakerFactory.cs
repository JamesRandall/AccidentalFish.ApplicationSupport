using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.CloudPatterns.Implementation
{
    internal class CircuitBreakerFactory : ICircuitBreakerFactory
    {
        private readonly ConcurrentDictionary<IFullyQualifiedName, ICircuitBreakerStateStore> _stateStores = new ConcurrentDictionary<IFullyQualifiedName, ICircuitBreakerStateStore>(); 

        public ICircuitBreaker Create(IFullyQualifiedName fullyQualifiedName)
        {
            ICircuitBreakerStateStore stateStore = _stateStores.GetOrAdd(fullyQualifiedName, fqn => new InMemoryCircuitBreakerStateStore());
            return new CircuitBreaker(stateStore);
        }
    }
}
