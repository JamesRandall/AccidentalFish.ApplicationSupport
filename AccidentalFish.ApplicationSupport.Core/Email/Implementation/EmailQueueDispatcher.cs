using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.Email.Implementation
{
    [ComponentIdentity(FullyQualifiedName)]
    internal class EmailQueueDispatcher : AbstractApplicationComponent, IEmailManager
    {
        public const string FullyQualifiedName = "com.accidental-fish.email";
        private readonly IAsynchronousQueue<EmailQueueItem> _queue; 

        public EmailQueueDispatcher(IApplicationResourceFactory applicationResourceFactory)
        {
            _queue = applicationResourceFactory.GetAsyncQueue<EmailQueueItem>(ComponentIdentity);
        }

        public Task SendAsync(string to, string cc, string @from, string emailTemplateId, Dictionary<string, string> mergeValues)
        {
            return SendAsync(new[] {to}, new[] {cc}, @from, emailTemplateId, mergeValues);
        }

        public async Task SendAsync(IEnumerable<string> to, IEnumerable<string> cc, string @from, string emailTemplateId, Dictionary<string, string> mergeValues)
        {
            EmailQueueItem item = new EmailQueueItem
            {
                Cc = new List<string>(cc),
                EmailTemplateId = emailTemplateId,
                From = from,
                MergeData = mergeValues,
                To = new List<string>(to)
            };

            await _queue.EnqueueAsync(item);
        }

        public Task SendAsync(string to, string cc, string @from, string subject, string htmlBody, string textBody)
        {
            return SendAsync(new[] { to }, new[] { cc }, @from, subject, htmlBody, textBody);
        }

        public async Task SendAsync(IEnumerable<string> to, IEnumerable<string> cc, string @from, string subject, string htmlBody, string textBody)
        {
            EmailQueueItem item = new EmailQueueItem
            {
                Cc = new List<string>(cc),
                From = from,
                To = new List<string>(to),
                Subject = subject,
                HtmlBody = htmlBody,
                TextBody = textBody
            };

            await _queue.EnqueueAsync(item);
        }
    }
}
