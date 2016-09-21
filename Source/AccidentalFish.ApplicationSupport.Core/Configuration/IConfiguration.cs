using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Interface for supplying global configuration information
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Storage account connection string
        /// </summary>
        string StorageAccountConnectionString { get; }
        /// <summary>
        /// Sql connection string
        /// </summary>
        string SqlConnectionString { get; }
        /// <summary>
        /// Service bus connection string
        /// </summary>
        string ServiceBusConnectionString { get; }
        /// <summary>
        /// Application setting
        /// </summary>
        /// <param name="key">Setting name</param>
        /// <returns>Setting value</returns>
        string this[string key] { get; }


        /// <summary>
        /// Application setting
        /// </summary>
        /// <param name="key">Setting name</param>
        /// <returns>Setting value</returns>
        [Obsolete("Preferred approach is to use the IAsyncConfiguration interface. This will be deprecated in an upcoming major version.")]
        Task<string> GetAsync(string key);
    }
}
