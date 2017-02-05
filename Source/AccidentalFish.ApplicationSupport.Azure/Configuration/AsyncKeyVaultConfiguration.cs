using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.KeyVault;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    internal class AsyncKeyVaultConfiguration : IAsyncKeyVaultConfiguration
    {
        private readonly IKeyVault _vault;
        private readonly IKeyVaultConfigurationKeyEncoder _keyEncoder;
        private readonly KeyVaultConfigurationCachePolicy _cachePolicy;
        private readonly IAsyncConfiguration _localConfiguration;
        private readonly ConcurrentDictionary<string, CachedApplicationSetting> _cachedSettings = new ConcurrentDictionary<string, CachedApplicationSetting>();
        
        /// <summary>
        /// Constructor for a key vault based setting cache
        /// </summary>
        /// <param name="vault">The key vault to use</param>
        /// <param name="keyEncoder">An encoder setting names (key vault doesn't support URI encoding for setting names but, for example, if migrating from app.config you could have a . in a setting name</param>
        /// <param name="cachePolicy">The policy for the cache</param>
        /// <param name="localConfiguration"></param>
        public AsyncKeyVaultConfiguration(
            IKeyVault vault,
            IKeyVaultConfigurationKeyEncoder keyEncoder,
            KeyVaultConfigurationCachePolicy cachePolicy,
            IAsyncConfiguration localConfiguration = null)
        {
            _vault = vault;
            _keyEncoder = keyEncoder;
            _cachePolicy = cachePolicy;

            _localConfiguration = localConfiguration;
        }

        public async Task<string> GetAsync(string key)
        {
            string value = null;
            CachedApplicationSetting cachedSetting;
            if (_cachedSettings.TryGetValue(key, out cachedSetting))
            {
                if (cachedSetting.ExpiresAt > DateTimeOffset.UtcNow)
                {
                    value = cachedSetting.Value;
                }
            }

            if (value == null)
            {
                if (_localConfiguration != null)
                {
                    value = await _localConfiguration.GetAsync(key);
                }
                if (string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        string keyVaultEncodedKey = _keyEncoder.Encode(key);
                        value = await _vault.GetSecretAsync(keyVaultEncodedKey);
                    }
                    catch (AggregateException ex)
                    {
                        KeyVaultErrorException kex = ex.InnerExceptions.FirstOrDefault() as KeyVaultErrorException;
                        if (kex != null && kex.Response.StatusCode == HttpStatusCode.NotFound)
                        {
                            value = null;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                if (_cachePolicy != null)
                {
                    if (value != null || _cachePolicy.CacheNotFoundAsNull)
                    {
                        CachedApplicationSetting newSetting = new CachedApplicationSetting
                        {
                            ExpiresAt = DateTimeOffset.UtcNow.Add(_cachePolicy.ExpiresAfter),
                            Key = key,
                            Value = value
                        };
                        _cachedSettings.AddOrUpdate(key, newSetting, (k, e) => newSetting);
                    }
                }
            }

            return value;
        }        
    }
}
