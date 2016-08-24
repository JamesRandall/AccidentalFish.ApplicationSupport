using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    public interface IKeyVaultConfiguration : IConfiguration
    {
        Task Preload();
    }
}
