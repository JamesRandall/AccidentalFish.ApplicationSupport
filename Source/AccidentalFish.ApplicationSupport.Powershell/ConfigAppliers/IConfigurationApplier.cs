using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Powershell.SecretStore;

namespace AccidentalFish.ApplicationSupport.Powershell.ConfigAppliers
{
    interface IConfigurationApplier
    {
        void Apply(ApplicationConfiguration configuration, ApplicationConfigurationSettings settings, string targetFile);
    }
}
