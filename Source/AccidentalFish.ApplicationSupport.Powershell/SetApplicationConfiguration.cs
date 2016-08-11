using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Powershell.ConfigAppliers;
using AccidentalFish.ApplicationSupport.Powershell.SecretStore;

namespace AccidentalFish.ApplicationSupport.Powershell
{
    [Cmdlet(VerbsCommon.Set, "ApplicationConfiguration")]
    public class SetApplicationConfiguration : AsyncPSCmdlet
    {
        private static readonly Dictionary<string, Type> FileProcessors = new Dictionary<string, Type>()
        {
            { ".config", typeof(DotNetConfigurationApplier) },
            { ".csdef", typeof(CsdefConfigurationApplier) },
            { ".cscfg", typeof(CscfgConfigurationApplier) },
            { ".js", typeof(JsConfigurationApplier) }
        };
            
        [Parameter(HelpMessage = "The application configuration file", Mandatory = true)]
        public string Configuration { get; set; }

        [Parameter(HelpMessage = "The application settings file to update - this can be a .cscfg, a .csdef or a .config file.", Mandatory = false)]
        public string Target { get; set; }

        [Parameter(HelpMessage = "Optional settings file(s). If more than one file is specified they are combined.", Mandatory = false)]
        public string[] Settings { get; set; }

        [Parameter(HelpMessage = "Defaults to false, if set to true then an exception will be thrown if a setting is required in a configuration file but not supplied", Mandatory = false)]
        public bool CheckForMissingSettings { get; set; }


        protected override async Task ProcessRecordAsync()
        {
            if (!File.Exists(Configuration))
            {
                throw new InvalidOperationException("Configuration file does not exist");
            }
            if (!File.Exists(Target))
            {
                throw new InvalidOperationException("Target does not exist");
            }

            ApplicationConfigurationSettings settings = Settings != null && Settings.Length > 0 ? ApplicationConfigurationSettings.FromFiles(Settings) :null;
            ApplicationConfiguration configuration = await ApplicationConfiguration.FromFileAsync(Configuration, settings, CheckForMissingSettings);

            if (!string.IsNullOrWhiteSpace(Target))
            {
                string extension = Path.GetExtension(Target).ToLower();
                Type configurationApplierType;
                if (!FileProcessors.TryGetValue(extension, out configurationApplierType))
                {
                    throw new InvalidOperationException("File extension not recognized");
                }
                IConfigurationApplier configurationApplier = (IConfigurationApplier)Activator.CreateInstance(configurationApplierType);
                configurationApplier.Apply(configuration, settings, Target);
            }            
        }
    }
}
