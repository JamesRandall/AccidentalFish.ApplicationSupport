using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Practices.ObjectBuilder2;

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
            element.Elements("cors-rule").ForEach(cr => CorsRules.Add(new ApplicationCorsRule(cr)));
        }

        public string Fqn { get; set; }

        public string ConnectionString { get; set; }

        public List<ApplicationCorsRule> CorsRules { get; set; } 
    }
}
