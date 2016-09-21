using System.Configuration;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.Azure;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    internal class AsyncConfiguration : IAsyncConfiguration
    {
        private readonly bool _forceAppConfig;

        public AsyncConfiguration(bool forceAppConfig)
        {
            _forceAppConfig = forceAppConfig;
        }

        public Task<string> GetAsync(string key)
        {
            if (!_forceAppConfig)
            {
                return Task.FromResult(CloudConfigurationManager.GetSetting(key));
            }
            return Task.FromResult(ConfigurationManager.AppSettings[key]);
        }
    }
}
