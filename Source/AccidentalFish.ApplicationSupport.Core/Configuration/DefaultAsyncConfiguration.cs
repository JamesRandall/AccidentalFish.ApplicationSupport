using System.Configuration;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    internal class DefaultAsyncConfiguration : IAsyncConfiguration
    {
        public Task<string> GetAsync(string key)
        {
            return Task.FromResult(ConfigurationManager.AppSettings[key]);
        }
    }
}
