using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Represents an application settings file
    /// </summary>
    public class ApplicationConfigurationSettings
    {
        private readonly Dictionary<string, ApplicationConfigurationSetting> _settings;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ApplicationConfigurationSettings()
        {
            _settings = new Dictionary<string, ApplicationConfigurationSetting>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ApplicationConfigurationSettings(IEnumerable<ApplicationConfigurationSetting> values)
        {
            _settings = values.ToDictionary(x => x.Key, x => x);
        }

        /// <summary>
        /// Create from the given file
        /// </summary>
        /// <param name="filename">Filename of the settings</param>
        /// <returns>Constructed ApplicationConfigurationSettings</returns>
        public static ApplicationConfigurationSettings FromFile(string filename)
        {
            ApplicationConfigurationSettings settings = new ApplicationConfigurationSettings();
            using (StreamReader streamReader = new StreamReader(filename))
            {
                XDocument document = XDocument.Load(streamReader);
                document.Root.Elements("setting").ToList().ForEach(element => settings._settings.Add(element.Attribute("key").Value, new ApplicationConfigurationSetting {
                    Key = element.Attribute("key").Value,
                    Value = element.Value,
                    IsSecret = element.Attribute("is-secret") != null
                }));
                return settings;
            }
        }

        /// <summary>
        /// Create and aggregate from the set of files supplied
        /// </summary>
        /// <param name="filenames">Filenames</param>
        /// <returns>Constructed ApplicationConfigurationSettings</returns>
        public static ApplicationConfigurationSettings FromFiles(string[] filenames)
        {
            ApplicationConfigurationSettings settings = new ApplicationConfigurationSettings();
            foreach (string filename in filenames)
            {
                using (StreamReader streamReader = new StreamReader(filename))
                {
                    XDocument document = XDocument.Load(streamReader);
                    document.Root.Elements("setting").ToList().ForEach(element => settings._settings[element.Attribute("key").Value] = new ApplicationConfigurationSetting
                    {
                        Key = element.Attribute("key").Value,
                        Value = element.Value,
                        IsSecret = element.Attribute("isSecret") != null
                    });
                }
            }
            return settings;
        }

        /// <summary>
        /// Create from the given set of setting values
        /// </summary>
        /// <param name="values">Collection of values</param>
        /// <returns>Constructed ApplicationConfigurationSettings</returns>
        public static ApplicationConfigurationSettings FromCollection(IEnumerable<ApplicationConfigurationSetting> values)
        {
            ApplicationConfigurationSettings settings = new ApplicationConfigurationSettings(values);
            return settings;
        }

        /// <summary>
        /// Replace tokenised settings in a configuration file with actual settings from this representation
        /// </summary>
        /// <param name="reader">Stream reader</param>
        /// <returns>New content</returns>
        public string Merge(StreamReader reader)
        {
            string content = reader.ReadToEnd();
            content = _settings.Aggregate(content, (current, kvp) => current.Replace(String.Format("{{{{{0}}}}}", kvp.Key), kvp.Value.Value));
            return content;
        }

        /// <summary>
        /// The settings
        /// </summary>
        public IReadOnlyDictionary<string, ApplicationConfigurationSetting> Settings => _settings;
    }
}
