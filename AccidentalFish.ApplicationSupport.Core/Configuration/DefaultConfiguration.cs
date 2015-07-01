using System.Configuration;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    internal class DefaultConfiguration : IConfiguration
    {
        public string StorageAccountConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["azure-storage-connection-string"];
            }
        }

        public string SqlConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["application-database"].ConnectionString;
            }
        }

        public string ServiceBusConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["service-bus-connection-string"];
            }
        }

        public string this[string key]
        {
            get
            {
                return ConfigurationManager.AppSettings[key];
            }
        }
    }
}
