using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Defines cors rules
    /// </summary>
    public class ApplicationCorsRule
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationCorsRule()
        {
            
        }

        /// <summary>
        /// Constructs cors rules from an XML element
        /// </summary>
        /// <param name="element"></param>
        public ApplicationCorsRule(XElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            AllowedOrigins = element.Element("allowed-origins").Value;
            AllowedVerbs = element.Element("allowed-methods").Value;
            AllowedHeaders = element.Element("allowed-headers").Value;
            ExposedHeaders = element.Element("exposed-headers").Value;
            MaxAgeSeconds = int.Parse(element.Element("max-age-seconds").Value);
        }

        /// <summary>
        /// Allowed origins
        /// </summary>
        public string AllowedOrigins { get; set; }

        /// <summary>
        /// Allowed verbs
        /// </summary>
        public string AllowedVerbs { get; set; }

        /// <summary>
        /// Allowed headers
        /// </summary>
        public string AllowedHeaders { get; set; }

        /// <summary>
        /// Exposed headers
        /// </summary>
        public string ExposedHeaders { get; set; }

        /// <summary>
        /// Max age in seconds
        /// </summary>
        public int MaxAgeSeconds { get; set; }

        /// <summary>
        /// Apply to tables
        /// </summary>
        public bool ApplyToTables { get; set; }

        /// <summary>
        /// Apply to blobs
        /// </summary>
        public bool ApplyToBlobs { get; set; }

        /// <summary>
        /// Apply to queues
        /// </summary>
        public bool ApplyToQueues { get; set; }
    }
}
