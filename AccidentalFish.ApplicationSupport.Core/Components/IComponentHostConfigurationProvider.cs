using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public interface IComponentHostConfigurationProvider
    {
        Task<IEnumerable<ComponentConfiguration>> GetConfiguration();
    }
}
