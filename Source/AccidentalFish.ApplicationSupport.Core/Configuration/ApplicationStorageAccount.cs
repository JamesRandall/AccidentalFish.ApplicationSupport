using System.Collections.Generic;
using System.Xml.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Represents an application storage account
    /// </summary>
    public class ApplicationStorageAccount
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationStorageAccount()
        {
            CorsRules = new List<ApplicationCorsRule>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element">XML element to construct the storage account from</param>
        public ApplicationStorageAccount(XElement element) : this()
        {
            Fqn = element.Element("fqn").Value;
            ConnectionString = element.Element("connection-string").Value;
            foreach (XElement cr in element.Elements("cors-rule"))
            {
                CorsRules.Add(new ApplicationCorsRule(cr));
            }
        }

        /// <summary>
        /// Fully qualified name
        /// </summary>
        public string Fqn { get; set; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Cors rules to apply
        /// </summary>
        public List<ApplicationCorsRule> CorsRules { get; set; } 
    }
}
