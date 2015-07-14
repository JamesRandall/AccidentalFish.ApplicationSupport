using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Email
{
    public class EmailQueueItem
    {
        public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public string From { get; set; }

        public Dictionary<string, string> MergeData { get; set; }

        public string EmailTemplateId { get; set; }

        public string Subject { get; set; }

        public string HtmlBody { get; set; }

        public string TextBody { get; set; }
    }
}
