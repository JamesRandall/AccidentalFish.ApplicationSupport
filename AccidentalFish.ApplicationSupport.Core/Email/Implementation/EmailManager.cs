using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.Email.Implementation
{
    [ComponentIdentity(FullyQualifiedName)]
    internal class EmailManager : AbstractApplicationComponent, IEmailManager
    {
        public const string FullyQualifiedName = "com.accidental-fish.email";
        private readonly IAsynchronousQueue<EmailQueueItem> _queue; 

        public EmailManager(IApplicationResourceFactory applicationResourceFactory)
        {
            _queue = applicationResourceFactory.GetQueue<EmailQueueItem>(ComponentIdentity);
        }

        public Task Send(string to, string cc, string @from, string emailTemplateId, Dictionary<string, string> mergeValues)
        {
            return Send(new[] {to}, new[] {cc}, @from, emailTemplateId, mergeValues);
        }

        public async Task Send(IEnumerable<string> to, IEnumerable<string> cc, string @from, string emailTemplateId, Dictionary<string, string> mergeValues)
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

        public Task Send(string to, string cc, string @from, string subject, string body)
        {
            return Send(new[] { to }, new[] { cc }, @from, subject, body);
        }

        public async Task Send(IEnumerable<string> to, IEnumerable<string> cc, string @from, string subject, string body)
        {
            EmailQueueItem item = new EmailQueueItem
            {
                Cc = new List<string>(cc),
                From = from,
                To = new List<string>(to),
                Subject = subject,
                Body = body
            };

            await _queue.EnqueueAsync(item);
        }
    }
}
