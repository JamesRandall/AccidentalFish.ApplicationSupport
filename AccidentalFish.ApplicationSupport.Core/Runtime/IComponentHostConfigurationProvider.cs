using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    public interface IComponentHostConfigurationProvider
    {
        Task<IEnumerable<ComponentConfiguration>> GetConfiguration();
    }
}
