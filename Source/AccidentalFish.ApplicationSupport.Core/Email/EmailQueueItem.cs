using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.Core.Templating;

namespace AccidentalFish.ApplicationSupport.Core.Email
{
    /// <summary>
    /// Queue item for an email
    /// </summary>
    public class EmailQueueItem
    {
        /// <summary>
        /// To recipients
        /// </summary>
        public List<string> To { get; set; }

        /// <summary>
        /// Cc recipients
        /// </summary>
        public List<string> Cc { get; set; }

        /// <summary>
        /// From address
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Merge data for the email if a template is being used
        /// </summary>
        public Dictionary<string, string> MergeData { get; set; }

        /// <summary>
        /// Name of the email template in the email blob store. If null then the Subject, HtmlBody and TextBody properties
        /// will be used.
        /// </summary>
        public string EmailTemplateId { get; set; }

        /// <summary>
        /// Subject for the email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Html body for the email
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// Plain text body for the email
        /// </summary>
        public string TextBody { get; set; }

        /// <summary>
        /// Email template syntax
        /// </summary>
        public TemplateSyntaxEnum? TemplateSyntax { get; set; }
    }
}
