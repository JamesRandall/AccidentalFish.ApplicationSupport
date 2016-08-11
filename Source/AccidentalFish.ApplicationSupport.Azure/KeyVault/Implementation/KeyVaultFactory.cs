namespace AccidentalFish.ApplicationSupport.Azure.KeyVault.Implementation
{
    class KeyVaultFactory : IKeyVaultFactory
    {
        public IKeyVault Create(string clientId, string clientSecret, string vaultUri, bool checkIfKeyExistsBeforeGet=false)
        {
            return new KeyVault(clientId, clientSecret, vaultUri, checkIfKeyExistsBeforeGet);
        }
    }
}
