using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    /// <summary>
    /// Cache policy for key vault configuration
    /// </summary>
    public class KeyVaultConfigurationCachePolicy
    {
        /// <summary>
        /// Is caching enabled
        /// </summary>
        public bool CachingEnabled { get; set; }

        /// <summary>
        /// Time after which the setting expires in the cache and is freshly fetched
        /// </summary>
        public TimeSpan ExpiresAfter { get; set; }

        /// <summary>
        /// If true then if an item is not found in the setting store it will be "saved" in the cache and returned as null
        /// until the item expires
        /// </summary>
        public bool CacheNotFoundAsNull { get; set; }

        /// <summary>
        /// Returns the default cache policy
        /// </summary>
        public static KeyVaultConfigurationCachePolicy Default => new KeyVaultConfigurationCachePolicy
        {
            CacheNotFoundAsNull = false,
            CachingEnabled = true,
            ExpiresAfter = TimeSpan.FromMinutes(5)
        };
    }
}
