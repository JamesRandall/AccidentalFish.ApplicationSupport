using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Extensions;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Templating;

namespace AccidentalFish.ApplicationSupport.Core.Email.Implementation
{
    [ComponentIdentity(FullyQualifiedName)]
#pragma warning disable 612
    internal class EmailQueueDispatcher : AbstractApplicationComponent, IEmailQueueDispatcher
#pragma warning restore 612
    {
        public const string FullyQualifiedName = "com.accidental-fish.email";
        private readonly IAsynchronousQueue<EmailQueueItem> _queue;
        private readonly ICoreAssemblyLogger _logger;

        public EmailQueueDispatcher(IApplicationResourceFactory applicationResourceFactory, ICoreAssemblyLogger logger)
        {
            _queue = applicationResourceFactory.GetAsyncQueue<EmailQueueItem>(ComponentIdentity);
            _logger = logger;
        }

        public Task SendAsync(string to, string cc, string @from, string emailTemplateId, Dictionary<string, string> mergeValues, TemplateSyntaxEnum templateSyntax = TemplateSyntaxEnum.Razor)
        {
            return SendAsync(new[] {to}, new[] {cc}, @from, emailTemplateId, mergeValues);
        }

        public async Task SendAsync(IEnumerable<string> to, IEnumerable<string> cc, string @from, string emailTemplateId, Dictionary<string, string> mergeValues, TemplateSyntaxEnum templateSyntax = TemplateSyntaxEnum.Razor)
        {
            _logger?.Verbose("EmailQueueDispatcher - SendAsync with template - {0}", emailTemplateId);

            EmailQueueItem item = new EmailQueueItem
            {
                Cc = new List<string>(cc),
                EmailTemplateId = emailTemplateId,
                From = from,
                MergeData = mergeValues,
                To = new List<string>(to),
                TemplateSyntax = templateSyntax
            };

            await _queue.EnqueueAsync(item);
        }

        public Task SendAsync(string to, string cc, string @from, string subject, string htmlBody, string textBody)
        {
            return SendAsync(new[] { to }, new[] { cc }, @from, subject, htmlBody, textBody);
        }

        public async Task SendAsync(IEnumerable<string> to, IEnumerable<string> cc, string @from, string subject, string htmlBody, string textBody)
        {
            _logger?.Verbose("EmailQueueDispatcher - SendAsync with subject/body - {0}", subject);

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
