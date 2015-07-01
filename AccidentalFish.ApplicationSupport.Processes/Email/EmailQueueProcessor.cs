using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
        private readonly IAsynchronousBackoffPolicy _backoffPolicy;
        private readonly IEmailProvider _emailProvider;
        private readonly IAsynchronousQueue<EmailQueueItem> _queue;
        private readonly IAsynchronousQueue<EmailQueueItem> _poisonQueue; 
        private readonly IAsynchronousBlockBlobRepository _blobRepository;
        private readonly ILogger _logger;
        private const int MaximumDeliveryAttempts = 5;

        public EmailQueueProcessor(IApplicationResourceFactory applicationResourceFactory,
            IAsynchronousBackoffPolicy backoffPolicy,
            ILoggerFactory loggerFactory,
            IEmailProvider emailProvider)
            : base(backoffPolicy, applicationResourceFactory.GetQueue<EmailQueueItem>(new ComponentIdentity(EmailFullyQualifiedName)))
        {
            IComponentIdentity emailComponentIdentity = new ComponentIdentity(EmailFullyQualifiedName);
            _backoffPolicy = backoffPolicy;
            _emailProvider = emailProvider;

            string poisonQueueName = applicationResourceFactory.Setting(emailComponentIdentity, "email-poison-queue");

            _queue = applicationResourceFactory.GetQueue<EmailQueueItem>(emailComponentIdentity);
            _poisonQueue = applicationResourceFactory.GetQueue<EmailQueueItem>(poisonQueueName, emailComponentIdentity);
            _blobRepository = applicationResourceFactory.GetBlockBlobRepository(emailComponentIdentity);
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
                    XDocument template = await GetTemplate(item.EmailTemplateId);
                    EmailContent content = ProcessTemplate(template, item.MergeData);
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

        public override IComponentIdentity ComponentIdentity
        {
            get { return new ComponentIdentity(EmailFullyQualifiedName); }
        }

        private async Task<XDocument> GetTemplate(string emailTemplateId)
        {
            if (!Path.HasExtension(emailTemplateId))
            {
                emailTemplateId = String.Format("{0}.xml", emailTemplateId);
            }
            IBlob blob = _blobRepository.Get(emailTemplateId);
            string template = await blob.DownloadStringAsync();
            XDocument xdoc = XDocument.Parse(template);
            return xdoc;
        }

        private EmailContent ProcessTemplate(XDocument template, Dictionary<string, string> mergeData)
        {
            string titleTemplate = template.Root.Element("title").Value;
            string bodyTemplate = template.Root.Element("body").Value;
            string title;
            string body;
            
            try
            {
                title = Razor.Parse(titleTemplate, mergeData);
            }
            catch (Exception ex)
            {
                _logger.Error("Error processing title template", ex);
                throw;
            }

            try
            {
                body = Razor.Parse(bodyTemplate, mergeData);
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
    }
}
