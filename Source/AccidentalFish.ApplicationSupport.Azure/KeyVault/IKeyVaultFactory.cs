namespace AccidentalFish.ApplicationSupport.Azure.KeyVault
{
    /// <summary>
    /// Interface for creating key vault access implementations
    /// </summary>
    public interface IKeyVaultFactory
    {
        /// <summary>
        /// Creates an interface to a key vault
        /// </summary>
        /// <param name="clientId">The client ID of ad Azure AD application associated with the key vault</param>
        /// <param name="clientSecret">The client secret of ad Azure AD application associated with the key vault</param>
        /// <param name="vaultUri">The URI of the key vault e.g. https://mykeyvault.vault.azure.net</param>
        /// <param name="checkIfKeyExistsBeforeGet">If true the key vault checks if the key exists before calling get. This is expensive but helps in Powershell.</param>
        /// <returns>Instance of an object with access to the key vault</returns>
        IKeyVault Create(string clientId, string clientSecret, string vaultUri, bool checkIfKeyExistsBeforeGet = false);
    }
}
