using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Powershell.ConfigAppliers
{
    interface IConfigurationApplier
    {
        void Apply(ApplicationConfiguration configuration, ApplicationConfigurationSettings settings, string targetFile);
    }
}
