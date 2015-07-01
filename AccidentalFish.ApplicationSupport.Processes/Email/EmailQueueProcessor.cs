using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using AccidentalFish.ApplicationSupport.Core.BackoffProcesses;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using RazorEngine;
using RazorEngine.Templating;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    [ComponentIdentity(HostableComponentNames.EmailQueueProcessor)]
    internal class EmailQueueProcessor : BackoffQueueProcessor<EmailQueueItem>
    {
        private class EmailContent
        {
            public string Title { get; set; }

            public string Body { get; set; }
        }

        private const string EmailFullyQualifiedName = "com.accidental-fish.email";
        private readonly IEmailProvider _emailProvider;
        private readonly IAsynchronousQueue<EmailQueueItem> _poisonQueue; 
        private readonly IAsynchronousBlockBlobRepository _blobRepository;
        private readonly ILogger _logger;
        private const int MaximumDeliveryAttempts = 5;

        public EmailQueueProcessor(IApplicationResourceFactory applicationResourceFactory,
            IAsynchronousBackoffPolicy backoffPolicy,
            ILoggerFactory loggerFactory,
            IEmailProvider emailProvider)
            : base(backoffPolicy, applicationResourceFactory.GetAsyncQueue<EmailQueueItem>(new ComponentIdentity(EmailFullyQualifiedName)))
        {
            IComponentIdentity emailComponentIdentity = new ComponentIdentity(EmailFullyQualifiedName);
            _emailProvider = emailProvider;

            string poisonQueueName = applicationResourceFactory.Setting(emailComponentIdentity, "email-poison-queue");

            _poisonQueue = applicationResourceFactory.GetAsyncQueue<EmailQueueItem>(poisonQueueName, emailComponentIdentity);
            _blobRepository = applicationResourceFactory.GetAsyncBlockBlobRepository(emailComponentIdentity);
            _logger = loggerFactory.CreateLongLivedLogger(emailComponentIdentity);
        }

        protected override async Task<bool> HandleRecievedItem(IQueueItem<EmailQueueItem> queueItem)
        {
            EmailQueueItem item = queueItem.Item;
            bool success;
            try
            {
                if (!String.IsNullOrWhiteSpace(item.EmailTemplateId))
                {
                    await GetTemplate(item.EmailTemplateId);
                    EmailContent content = ProcessTemplate(item.EmailTemplateId, item.MergeData);
                    _emailProvider.Send(item.To, item.Cc, item.From, content.Title, content.Body);
                }
                else
                {
                    _emailProvider.Send(item.To, item.Cc, item.From, item.Subject, item.Body);
                }
                
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error sending email", ex).Wait();
                success = false;
            }

            if (!success && queueItem.DequeueCount >= MaximumDeliveryAttempts)
            {
                success = true;
                try
                {
                    await _poisonQueue.EnqueueAsync(item);
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            
            return success;
        }

        private async Task GetTemplate(string emailTemplateId)
        {
            if (Engine.Razor.IsTemplateCached(TitleTemplateCacheName(emailTemplateId), typeof (Dictionary<string, string>)))
            {
                return;
            }
            
            if (!Path.HasExtension(emailTemplateId))
            {
                emailTemplateId = String.Format("{0}.xml", emailTemplateId);
            }
            IBlob blob = _blobRepository.Get(emailTemplateId);
            string template = await blob.DownloadStringAsync();
            XDocument xdoc = XDocument.Parse(template);

            string titleTemplate = xdoc.Root.Element("title").Value;
            string bodyTemplate = xdoc.Root.Element("body").Value;

            Engine.Razor.AddTemplate(TitleTemplateCacheName(emailTemplateId), titleTemplate);
            Engine.Razor.AddTemplate(BodyTemplateCacheName(emailTemplateId), bodyTemplate);
            Engine.Razor.Compile(TitleTemplateCacheName(emailTemplateId), typeof(Dictionary<string, string>));
            Engine.Razor.Compile(BodyTemplateCacheName(emailTemplateId), typeof(Dictionary<string, string>));
        }

        private string TitleTemplateCacheName(string emailTemplateId)
        {
            return String.Format("{0}-title", emailTemplateId);
        }

        private string BodyTemplateCacheName(string emailTemplateId)
        {
            return String.Format("{0}-body", emailTemplateId);
        }

        private EmailContent ProcessTemplate(string emailTemplateId, Dictionary<string, string> mergeData)
        {
            
            string title;
            string body;
            
            try
            {

                title = Engine.Razor.Run(
                    TitleTemplateCacheName(emailTemplateId),
                    typeof (Dictionary<string, string>),
                    mergeData);
            }
            catch (Exception ex)
            {
                _logger.Error("Error processing title template", ex);
                throw;
            }

            try
            {
                body = Engine.Razor.Run(
                    BodyTemplateCacheName(emailTemplateId),
                    typeof(Dictionary<string, string>),
                    mergeData);
            }
            catch (Exception ex)
            {
                _logger.Error("Error processing body template", ex);
                throw;
            }

            return new EmailContent
            {
                Body = body,
                Title = title
            };
        }

        public override IComponentIdentity ComponentIdentity
        {
            get { return new ComponentIdentity(HostableComponentNames.EmailQueueProcessor); }
        }
    }
}
