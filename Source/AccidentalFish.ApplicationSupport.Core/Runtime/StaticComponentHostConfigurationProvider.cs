using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    /// <summary>
    /// Use this class to provide component configurations to the component host
    /// </summary>
    public class StaticComponentHostConfigurationProvider : IComponentHostConfigurationProvider
    {
        private readonly IEnumerable<ComponentConfiguration> _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">A collection of component configurations</param>
        public StaticComponentHostConfigurationProvider(IEnumerable<ComponentConfiguration> configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns the configuration
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ComponentConfiguration>> GetConfigurationAsync()
        {
            return Task.FromResult(_configuration);
        }
    }
}
