using System.Collections.Generic;
using System.Linq;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Core.Email.Providers
{
    class AmazonSimpleEmailProvider : IEmailProvider
    {
        private readonly string _accessKey;
        private readonly string _secretKey;

        public AmazonSimpleEmailProvider(IConfiguration configuration)
        {
            Condition.Requires(configuration, "configuration").IsNotNull();

            _accessKey = configuration["amazon-access-key"];
            _secretKey = configuration["amazon-secret-key"];
        }

        public string Send(IEnumerable<string> to, IEnumerable<string> cc, string from, string title, string body)
        {
            AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(_accessKey, _secretKey);
            Destination destination = new Destination(to.ToList());
            Content subject = new Content(title);
            Body bodyContent = new Body(new Content(body));
            Message message = new Message(subject, bodyContent);
            SendEmailRequest request = new SendEmailRequest(from, destination, message);
            SendEmailResponse response = client.SendEmail(request);
            return response.SendEmailResult.MessageId;
        }
    }
}
