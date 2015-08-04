using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.ApplicationSupport.Core.Alerts.Implementation
{
    [ComponentIdentity(FullyQualifiedName)]
    internal class AlertSender : AbstractApplicationComponent, IAlertSender
    {
        private readonly IEmailProvider _emailProvider;
        public const string FullyQualifiedName = "com.accidentalfish.alert-sender";

        private readonly IAsynchronousNoSqlRepository<AlertSubscriber> _table;
        private readonly string _sourceEmailAddress;
        private readonly ILogger _logger;

        public AlertSender(
            ILoggerFactory loggerFactory,
            IApplicationResourceFactory applicationResourceFactory,
            IEmailProvider emailProvider)
        {
            _emailProvider = emailProvider;
            _table = applicationResourceFactory.GetNoSqlRepository<AlertSubscriber>(ComponentIdentity);
            _logger = loggerFactory.CreateShortLivedLogger(ComponentIdentity);
            _sourceEmailAddress = applicationResourceFactory.Setting(ComponentIdentity, "alert-from");
        }

        public async void Send(string title, string message)
        {
            List<AlertSubscriber> subscribers;
            try
            {
                subscribers = new List<AlertSubscriber>(await _table.GetAsync("v1.0.0.0"));
            }
            catch (Exception ex)
            {
                _logger.Warning("Unable to retrieve alert subscribers", ex).Wait();
                return;
            }

            if (!subscribers.Any())
            {
                _logger.Information("No alert subscribers are configured").Wait();
                return;
            }

            List<string> emailAddresses = subscribers.Select(x => x.Email).ToList();
            try
            {
                _emailProvider.Send(emailAddresses, null, _sourceEmailAddress, title, message, null);
            }
            catch (Exception)
            {
                // send a warning and not an error as a warning will just cause another alert to be sent, then another etc.
                // and if alerts can't be received...
                _logger.Warning("Unable to send email alert").Wait();
            }
            
        }
    }
}
