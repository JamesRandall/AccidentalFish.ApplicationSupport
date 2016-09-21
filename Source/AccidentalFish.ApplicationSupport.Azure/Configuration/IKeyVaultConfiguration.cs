using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    [Obsolete("The preferred approach is to use IAsyncKeyVaultConfiguration, this interface will be deprecated")]
    public interface IKeyVaultConfiguration : IConfiguration
    {
        Task Preload();
    }
}
