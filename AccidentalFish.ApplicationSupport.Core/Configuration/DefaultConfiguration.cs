using System.Configuration;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Returns configurations from app.config / web.config files
    /// </summary>
    internal class DefaultConfiguration : IConfiguration
    {
        /// <summary>
        /// Storage account connection string - held in the app setting "azure-storage-connection-string"
        /// </summary>
        public string StorageAccountConnectionString => ConfigurationManager.AppSettings["azure-storage-connection-string"];

        /// <summary>
        /// Sql connection string - held in the app setting "application-database"
        /// </summary>
        public string SqlConnectionString => ConfigurationManager.ConnectionStrings["application-database"].ConnectionString;

        /// <summary>
        /// Service bus connection string - held in the app setting "service-bus-connection-string"
        /// </summary>
        public string ServiceBusConnectionString => ConfigurationManager.AppSettings["service-bus-connection-string"];

        /// <summary>
        /// Returns application settings
        /// </summary>
        /// <param name="key">Key for the setting</param>
        /// <returns>Setting value</returns>
        public string this[string key] => ConfigurationManager.AppSettings[key];
    }
}
