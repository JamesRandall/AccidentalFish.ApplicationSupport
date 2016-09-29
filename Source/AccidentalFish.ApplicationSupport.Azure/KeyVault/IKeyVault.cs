using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Azure.KeyVault
{
    public interface IKeyVault : IApplicationSecretStore
    {
        Task<IReadOnlyCollection<string>> GetSecretKeysAsync();
    }
}
