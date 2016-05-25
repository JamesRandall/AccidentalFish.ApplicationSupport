using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Configuration
{
    [TestClass]
    public class ApplicationConfigurationTests
    {
        private XDocument GetEmbeddedConfigurationFile()
        {
            using (
                StreamReader streamReader =
                    new StreamReader(
                        GetType()
                            .Assembly.GetManifestResourceStream(
                                "AccidentalFish.ApplicationSupport.Core.Tests.Unit.Configuration.configuration.xml")))
            {
                string asString = streamReader.ReadToEnd();
                return XDocument.Parse(asString);
            }
        }

        [TestMethod]
        public void ReplacesSettings()
        {
            // Arrange
            XDocument configurationDocument = GetEmbeddedConfigurationFile();
            ApplicationConfigurationSettings settings = ApplicationConfigurationSettings.FromCollection(new List<ApplicationConfigurationSetting>
            {
                new ApplicationConfigurationSetting { IsSecret = false, Key = "service-bus-connection-string", Value = "HelloWorld"},
                new ApplicationConfigurationSetting { IsSecret = false, Key = "this", Value = "***"},
                new ApplicationConfigurationSetting { IsSecret = false, Key = "message", Value = "###"}
            });

            // Act
            ApplicationConfiguration configuration = ApplicationConfiguration.FromXDocument(configurationDocument, settings, true);

            // Assert
            ApplicationComponent component = configuration.ApplicationComponents.Single(x => x.Fqn == "accidentalfish.samples.topicsandsubscriptions.processor");
            Assert.AreEqual("*** is a ###", component.Settings.Single(x => x.Key == "somesetting").Value);
            Assert.AreEqual("HelloWorld", configuration.ServiceBusConnectionStrings["accidentalfish.samples.topicsandsubscriptions.servicebus"]);
        }

        [TestMethod]
        public void SecretIsCollected()
        {
            // Arrange
            XDocument configurationDocument = GetEmbeddedConfigurationFile();
            ApplicationConfigurationSettings settings = ApplicationConfigurationSettings.FromCollection(new List<ApplicationConfigurationSetting>
            {
                new ApplicationConfigurationSetting { IsSecret = false, Key = "service-bus-connection-string", Value = "HelloWorld"},
                new ApplicationConfigurationSetting { IsSecret = true, Key = "this", Value = "***"},
                new ApplicationConfigurationSetting { IsSecret = false, Key = "message", Value = "###"}
            });

            // Act
            ApplicationConfiguration configuration = ApplicationConfiguration.FromXDocument(configurationDocument, settings, true);

            // Assert
            Assert.AreEqual("*** is a ###", configuration.Secrets.Single());
        }

        [TestMethod]
        [ExpectedException(typeof(MissingSettingException))]
        public void ThrowsMissingSettingException()
        {
            // Arrange
            XDocument configurationDocument = GetEmbeddedConfigurationFile();
            ApplicationConfigurationSettings settings = ApplicationConfigurationSettings.FromCollection(new List<ApplicationConfigurationSetting>
            {
                
            });

            // Act
            ApplicationConfiguration configuration = ApplicationConfiguration.FromXDocument(configurationDocument, settings, true);            
        }
    }
}
