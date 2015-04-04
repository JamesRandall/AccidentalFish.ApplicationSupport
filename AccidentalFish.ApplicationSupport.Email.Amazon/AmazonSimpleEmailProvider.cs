using System;
using System.Collections.Generic;
using System.Linq;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Email;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace AccidentalFish.ApplicationSupport.Email.Amazon
{

    internal class AmazonSimpleEmailProvider : AbstractApplicationComponent, IEmailProvider
    {
        public const string FullyQualifiedName = "";
        private readonly string _accessKey;
        private readonly string _secretKey;

        public AmazonSimpleEmailProvider(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _accessKey = configuration["amazon-access-key"];
            _secretKey = configuration["amazon-secret-key"];
        }

        public string Send(IEnumerable<string> to, IEnumerable<string> cc, string from, string title, string body)
        {
            AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(_accessKey, _secretKey);
            Destination destination = new Destination();
            destination.ToAddresses = to.ToList();
            Content subject = new Content(title);
            Body bodyContent = new Body(new Content(body));
            Message message = new Message(subject, bodyContent);
            SendEmailRequest request = new SendEmailRequest
            {
                ReplyToAddresses = new List<string>() {@from},
                Destination = destination,
                Message = message
            };
            SendEmailResponse response = client.SendEmail(request);
            return response.MessageId;
        }
    }
}
