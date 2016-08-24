using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.KeyVault;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.Azure.KeyVault;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    /// <summary>
    /// This class needs to be thread safe.
    /// </summary>
    internal class KeyVaultConfiguration : IKeyVaultConfiguration
    {
        private readonly IKeyVault _vault;
        private readonly IKeyVaultConfigurationKeyEncoder _keyEncoder;
        private readonly IConfiguration _localConfiguration;
        private readonly ConcurrentDictionary<string, string> _keyedSettings = new ConcurrentDictionary<string, string>();
        private bool _isPreloaded;

        public KeyVaultConfiguration(IKeyVault vault, IKeyVaultConfigurationKeyEncoder keyEncoder, IConfiguration localConfiguration = null)
        {
            _vault = vault;
            _keyEncoder = keyEncoder;
            _localConfiguration = localConfiguration;
        }

        public string StorageAccountConnectionString => this["azure-storage-connection-string"];
        public string SqlConnectionString => this["application-database"];
        public string ServiceBusConnectionString => this["service-bus-connection-string"];

        public async Task<string> GetAsync(string key)
        {            
            string value;

            if (!_keyedSettings.TryGetValue(key, out value))
            {
                if (_localConfiguration != null)
                {
                    value = _localConfiguration[key];
                }
                if (string.IsNullOrWhiteSpace(value) && !_isPreloaded)
                {
                    try
                    {
                        string keyVaultEncodedKey = _keyEncoder.Encode(key);
                        value = await _vault.GetSecretAsync(keyVaultEncodedKey);
                    }
                    catch (AggregateException ex)
                    {
                        KeyVaultClientException kex = ex.InnerExceptions.FirstOrDefault() as KeyVaultClientException;
                        if (kex != null && kex.Status == HttpStatusCode.NotFound)
                        {
                            value = null;
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
                _keyedSettings.AddOrUpdate(key, value, (s, s1) => value);
            }
            return value;
        }

        public async Task Preload()
        {
            IReadOnlyCollection<string> keys = await _vault.GetSecretKeysAsync();
            foreach (string key in keys)
            {
                string value = await _vault.GetSecretAsync(key);
                string decodedKey = _keyEncoder.Decode(key);
                _keyedSettings.AddOrUpdate(decodedKey, value, (s, s1) => value);
            }
            _isPreloaded = true;
        }

        public string this[string key] => GetAsync(key).Result;
    }
}
