using CommandLine;
using CommandLine.Text;

namespace SetKeyVaultSecrets
{
    internal class Options
    {
        [Option('c', "configuration", HelpText = "The application configuration file", Required = true)]
        public string Configuration { get; set; }

        [Option('s', "settings", HelpText = "Settings file(s). If more than one file is specified they are combined. Separate multiple setting files with a comma.", Required = true)]
        public string Settings { get; set; }

        [Option('i', "keyvaultclientid", HelpText = "Client ID for the key vault to use for secrets. The service principal must have Set rights for key vault secrets.", Required = true)]
        public string KeyVaultClientId { get; set; }

        [Option('k', "keyvaultclientkey", HelpText = "Client key for the key vault to use for secrets.", Required = true)]
        public string KeyVaultClientKey { get; set; }

        [Option('u', "keyvaulturi", HelpText = "Uri of the key vault to use for secrets.", Required = true)]
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
