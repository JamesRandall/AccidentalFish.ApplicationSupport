using System.Collections.Generic;
using System.Xml.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    public class ApplicationStorageAccount
    {
        public ApplicationStorageAccount()
        {
            CorsRules = new List<ApplicationCorsRule>();
        }

        public ApplicationStorageAccount(XElement element) : this()
        {
            Fqn = element.Element("fqn").Value;
            ConnectionString = element.Element("connection-string").Value;
            foreach (XElement cr in element.Elements("cors-rule"))
            {
                CorsRules.Add(new ApplicationCorsRule(cr));
            }
        }

        public string Fqn { get; set; }

        public string ConnectionString { get; set; }

        public List<ApplicationCorsRule> CorsRules { get; set; } 
    }
}
