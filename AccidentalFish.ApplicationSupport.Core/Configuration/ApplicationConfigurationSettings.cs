using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    public class ApplicationConfigurationSettings
    {
        private readonly Dictionary<string, string> _settings;

        protected ApplicationConfigurationSettings()
        {
            _settings = new Dictionary<string, string>();
        }

        public static ApplicationConfigurationSettings FromFile(string filename)
        {
            ApplicationConfigurationSettings settings = new ApplicationConfigurationSettings();
            using (StreamReader streamReader = new StreamReader(filename))
            {
                XDocument document = XDocument.Load(streamReader);
                document.Root.Elements("setting").ToList().ForEach(element => settings._settings.Add(element.Attribute("key").Value, element.Value));
                return settings;
            }
        }

        public string Merge(StreamReader reader)
        {
            string content = reader.ReadToEnd();
            content = _settings.Aggregate(content, (current, kvp) => current.Replace(String.Format("{{{{{0}}}}}", kvp.Key), kvp.Value));
            return content;
        }
    }
}
