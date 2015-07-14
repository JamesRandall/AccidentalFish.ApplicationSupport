using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Email;
using SendGrid;

namespace AccidentalFish.ApplicationSupport.Email.SendGrid
{
    [ComponentIdentity(FullyQualifiedName)]
    internal class SendGridEmailProvider : AbstractApplicationComponent, IEmailProvider
    {
        public const string FullyQualifiedName = "com.accidentalfish.sendgrid";
        private readonly string _sendgridUsername;
        private readonly string _sendgridPassword;

        public SendGridEmailProvider(IApplicationResourceFactory applicationResourceFactory)
        {
            if (applicationResourceFactory == null) throw new ArgumentNullException("applicationResourceFactory");
            _sendgridUsername = applicationResourceFactory.Setting(ComponentIdentity, "username");
            _sendgridPassword = applicationResourceFactory.Setting(ComponentIdentity, "password");
        }

        public string Send(IEnumerable<string> to, IEnumerable<string> cc, string @from, string title, string htmlBody, string textBody)
        {
            SendGridMessage message = new SendGridMessage();
            message.From = new MailAddress(@from);
            if (to != null && to.Any())
            {
                message.AddTo(to);
            }
            if (cc != null && cc.Any())
            {
                message.AddTo(cc);
            }
            if (htmlBody != null)
                message.Html = htmlBody;
            if (textBody != null)
                message.Text = textBody;
            message.Subject = title;

            NetworkCredential credentials = new NetworkCredential(_sendgridUsername, _sendgridPassword);
            Web transportWeb = new Web(credentials);
            transportWeb.Deliver(message);
            return null;
        }
    }
}
