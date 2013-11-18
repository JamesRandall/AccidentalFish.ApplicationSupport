using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using CuttingEdge.Conditions;
using SendGrid;
using SendGrid.Transport;

namespace AccidentalFish.ApplicationSupport.Core.Email.Providers
{
    internal class SendGridEmailProvider : IEmailProvider
    {
        private readonly string _sendgridUsername;
        private readonly string _sendgridPassword;

        public SendGridEmailProvider(IConfiguration configuration)
        {
            Condition.Requires(configuration).IsNotNull();
            _sendgridUsername = configuration["sendgrid-username"];
            _sendgridPassword = configuration["sendgrid-password"];
        }

        public string Send(IEnumerable<string> to, IEnumerable<string> cc, string @from, string title, string body)
        {
            Mail mail = Mail.GetInstance();
            if (to != null && to.Any())
            {
                mail.AddTo(to);
            }
            if (cc != null && cc.Any())
            {
                mail.AddCc(cc);
            }
            mail.From = new MailAddress(@from);
            mail.Html = body;
            mail.Subject = title;

            var transportInstance = Web.GetInstance(new NetworkCredential(_sendgridUsername, _sendgridPassword));
            transportInstance.Deliver(mail);
            return null;
        }
    }
}
