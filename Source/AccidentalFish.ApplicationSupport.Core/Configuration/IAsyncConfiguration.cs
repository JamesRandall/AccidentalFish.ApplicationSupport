using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Interface for supplying global configuration information
    /// </summary>
    public interface IAsyncConfiguration
    {
        /// <summary>
        /// Application setting
        /// </summary>
        /// <param name="key">Setting name</param>
        /// <returns>Setting value</returns>
        Task<string> GetAsync(string key);
    }
}
