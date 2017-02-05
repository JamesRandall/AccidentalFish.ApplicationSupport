using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.Azure;

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
                SecretBundle secret = await _keyVaultClient.GetSecretAsync(_vaultUri, key);
                return secret.Value;
            }
            catch (KeyVaultErrorException kex)
            {                
                if (kex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }            
        }

        public async Task<IReadOnlyCollection<string>> GetSecretKeysAsync()
        {
            List<string> results = new List<string>();
            IPage<SecretItem> response = await _keyVaultClient.GetSecretsAsync(_vaultUri);
            results.AddRange(response.Select(x => x.Identifier.Name));

            while (!string.IsNullOrWhiteSpace(response.NextPageLink))
            {
                response = await _keyVaultClient.GetSecretsNextAsync(response.NextPageLink);
                results.AddRange(response.Select(x => x.Identifier.Name));
            } 
            return results;
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
