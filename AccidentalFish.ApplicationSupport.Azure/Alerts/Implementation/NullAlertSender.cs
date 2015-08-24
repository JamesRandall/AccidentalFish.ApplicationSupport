using AccidentalFish.ApplicationSupport.Core.Alerts;

namespace AccidentalFish.ApplicationSupport.Azure.Alerts.Implementation
{
    class NullAlertSender : IAlertSender
    {
        public void Send(string title, string message)
        {
            
        }
    }
}
