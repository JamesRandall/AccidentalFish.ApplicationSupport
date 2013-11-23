using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    public class StaticComponentHostConfigurationProvider : IComponentHostConfigurationProvider
    {
        private readonly IEnumerable<ComponentConfiguration> _configuration;

        public StaticComponentHostConfigurationProvider(IEnumerable<ComponentConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public Task<IEnumerable<ComponentConfiguration>> GetConfiguration()
        {
            return Task.FromResult(_configuration);
        }
    }
}
