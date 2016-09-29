using System.Collections.Generic;
using System.IO;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Powershell.ConfigAppliers
{
    class JsConfigurationApplier : IConfigurationApplier
    {
        public void Apply(ApplicationConfiguration configuration, ApplicationConfigurationSettings settings, string targetFile)
        {
            string contents;
            using (StreamReader reader = new StreamReader(targetFile))
            {
                contents = reader.ReadToEnd();
            }

            foreach (KeyValuePair<string, ApplicationConfigurationSetting> setting in settings.Settings)
            {
                contents = contents.Replace("{{" + setting.Key + "}}", setting.Value.Value);
            }

            using (StreamWriter writer = new StreamWriter(targetFile, false))
            {
                writer.Write(contents);
                writer.Flush();
            }
        }
    }
}
