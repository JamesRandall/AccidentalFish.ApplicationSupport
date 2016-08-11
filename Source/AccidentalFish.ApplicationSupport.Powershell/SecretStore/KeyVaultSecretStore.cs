using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AccidentalFish.ApplicationSupport.Powershell.SecretStore
{
    internal class KeyVaultSecretStore : ISecretStore
    {
        private readonly string _vaultName;
        private readonly string _clientId;
        private readonly string _clientKey;
        private readonly KeyVaultClient _keyVaultClient;

        public KeyVaultSecretStore(string vaultName, string clientId, string clientKey)
        {
            _vaultName = vaultName;
            _clientId = clientId;
            _clientKey = clientKey;

            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
        }

        public string EncodeKey(string key)
        {
            return key.Replace(".", "AFDOT").Replace("-", "AFDASH");
        }

        public async Task Save(string key, string value)
        {
            string encodedKey = EncodeKey(key);
            await _keyVaultClient.SetSecretAsync(_vaultName, encodedKey, value);
        }

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(_clientId, _clientKey);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}
