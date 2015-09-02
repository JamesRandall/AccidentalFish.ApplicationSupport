using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Alerts
{
    /// <summary>
    /// Send an alert to alert subscribers
    /// </summary>
    public interface IAlertSender
    {
        /// <summary>
        /// Sends an alert to all system alert subscribers
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message body of the alert</param>
        /// <returns></returns>
        Task SendAsync(string title, string message);
    }
}
