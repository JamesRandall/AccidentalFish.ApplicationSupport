﻿namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    public interface IKeyVaultConfigurationKeyEncoder
    {
        string Encode(string key);
    }
}