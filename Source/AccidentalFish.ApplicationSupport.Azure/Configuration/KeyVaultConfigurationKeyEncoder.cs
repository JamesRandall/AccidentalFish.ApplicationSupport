namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    class KeyVaultConfigurationKeyEncoder : IKeyVaultConfigurationKeyEncoder
    {
        public string Encode(string key)
        {
            return key.Replace(".", "AFDOT").Replace("-", "AFDASH");
        }
    }
}
