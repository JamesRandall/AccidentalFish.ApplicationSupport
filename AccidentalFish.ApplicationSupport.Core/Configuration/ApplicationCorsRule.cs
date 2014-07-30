using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    public class ApplicationCorsRule
    {
        public ApplicationCorsRule()
        {
            
        }

        public ApplicationCorsRule(XElement element)
        {
            AllowedOrigins = element.Element("allowed-origins").Value;
            AllowedVerbs = element.Element("allowed-methods").Value;
            AllowedHeaders = element.Element("allowed-headers").Value;
            ExposedHeaders = element.Element("exposed-headers").Value;
            MaxAgeSeconds = int.Parse(element.Element("max-age-seconds").Value);
        }

        public string AllowedOrigins { get; set; }

        public string AllowedVerbs { get; set; }

        public string AllowedHeaders { get; set; }

        public string ExposedHeaders { get; set; }

        public int MaxAgeSeconds { get; set; }
        public bool ApplyToTables { get; set; }

        public bool ApplyToBlobs { get; set; }

        public bool ApplyToQueues { get; set; }
    }
}
