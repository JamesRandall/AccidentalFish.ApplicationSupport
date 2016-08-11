﻿using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// An abstraction for stores that can contain application secrets
    /// </summary>
    public interface IApplicationSecretStore
    {
        /// <summary>
        /// Retrieves a secret
        /// </summary>
        /// <param name="key">Key / name of the secret</param>
        /// <returns>Value of the secret</returns>
        Task<string> GetSecretAsync(string key);
    }
}
