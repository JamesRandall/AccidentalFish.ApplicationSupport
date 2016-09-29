using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace NewApplicationResources
{
    internal class Options
    {
        [Option('c', "configuration", HelpText = "The application configuration file", Required = true)]
        public string Configuration { get; set; }

        [Option('m', "checkformissingsettings", DefaultValue = false, HelpText = "Defaults to false, if set to true then an exception will be thrown if a setting is required in a configuration file but not supplied", Required = false)]
        public bool CheckForMissingSettings { get; set; }

        [Option('s', "settings", HelpText = "Settings file(s). If more than one file is specified they are combined. Separate multiple setting files with a comma.", Required = true)]
        public string Settings { get; set; }

        [Option('i', "keyvaultclientid", HelpText = "Client ID for the key vault to use for secrets. The service principal must have Set rights for key vault secrets.", Required = false)]
        public string KeyVaultClientId { get; set; }

        [Option('k', "keyvaultclientkey", HelpText = "Client key for the key vault to use for secrets.", Required = false)]
        public string KeyVaultClientKey { get; set; }

        [Option('u', "keyvaulturi", HelpText = "Uri of the key vault to use for secrets.", Required = false)]
        public string KeyVaultUri { get; set; }

        [Option('v', "verbose", HelpText = "Verbose output", Required = false, DefaultValue = false)]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
