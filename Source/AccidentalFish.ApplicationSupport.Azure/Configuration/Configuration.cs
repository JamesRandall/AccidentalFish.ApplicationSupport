using System.Configuration;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.Azure;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    public class Configuration : IConfiguration
    {
        private readonly bool _forceAppConfig;

        public Configuration(bool forceAppConfig)
        {
            _forceAppConfig = forceAppConfig;
        }

        public string StorageAccountConnectionString
        {
            get
            {
                if (!_forceAppConfig)
                {
                    return CloudConfigurationManager.GetSetting("azure-storage-connection-string");                
                }
                return ConfigurationManager.AppSettings["azure-storage-connection-string"];
            }
        }

        public string SqlConnectionString
        {
            get
            {
                if (!_forceAppConfig)
                {
                    return CloudConfigurationManager.GetSetting("application-database");
                }
                return ConfigurationManager.ConnectionStrings["application-database"].ConnectionString;
            }
        }

        public string ServiceBusConnectionString
        {
            get
            {
                if (!_forceAppConfig)
                {
                    return CloudConfigurationManager.GetSetting("service-bus-connection-string");
                }
                return ConfigurationManager.AppSettings["service-bus-connection-string"];
            }
        }

        public string this[string key]
        {
            get
            {
                if (!_forceAppConfig)
                {
                    return CloudConfigurationManager.GetSetting(key);
                }
                return ConfigurationManager.AppSettings[key];
            }
        }

        public Task<string> GetAsync(string key)
        {
            return Task.FromResult(this[key]);
        }
    }
}
