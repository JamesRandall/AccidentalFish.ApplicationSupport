using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AccidentalFish.ApplicationSupport.Azure.KeyVault.Implementation
{
    internal class KeyVault : IKeyVault
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _vaultUri;
        private readonly bool _checkIfKeyExistsBeforeGet;
        private readonly KeyVaultClient _keyVaultClient;

        public KeyVault(string clientId, string clientSecret, string vaultUri, bool checkIfKeyExistsBeforeGet)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _vaultUri = vaultUri;
            _checkIfKeyExistsBeforeGet = checkIfKeyExistsBeforeGet;
            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
        }

        public async Task SetSecretAsync(string key, string value)
        {
            await _keyVaultClient.SetSecretAsync(_vaultUri, key, value);
        }

        public async Task<string> GetSecretAsync(string key)
        {
            try
            {
                if (_checkIfKeyExistsBeforeGet)
                {
                    IReadOnlyCollection<string> keys = await GetSecretKeysAsync();
                    if (!keys.Contains(key))
                    {
                        return null;
                    }
                }
                Secret secret = await _keyVaultClient.GetSecretAsync(_vaultUri, key);
                return secret.Value;
            }
            catch (KeyVaultClientException kex)
            {
                if (kex.Error.Code == "SecretNotFound")
                {
                    return null;
                }
                throw;
            }            
        }

        public async Task<IReadOnlyCollection<string>> GetSecretKeysAsync()
        {
            ListSecretsResponseMessage response = await _keyVaultClient.GetSecretsAsync(_vaultUri);
            return response.Value.Select(x => x.Identifier.Name).ToList();
        }

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(_clientId, _clientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}
