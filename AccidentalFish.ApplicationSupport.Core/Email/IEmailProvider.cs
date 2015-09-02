using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Email
{
    /// <summary>
    /// Interface for an email dispatch provider such as SendGrid or Amazon SES
    /// </summary>
    public interface IEmailProvider
    {
        /// <summary>
        /// Send an email to the dispatcher
        /// </summary>
        /// <param name="to">The to addresses</param>
        /// <param name="cc">The cc addresses</param>
        /// <param name="from">The from address</param>
        /// <param name="title">The title of the email</param>
        /// <param name="htmlBody">The HTML body of the email</param>
        /// <param name="textBody">The text body of the email</param>
        /// <returns>An awaitable task with an optional ID for the email where the provider supports tracking by ID</returns>
        Task<string> SendAsync(IEnumerable<string> to, IEnumerable<string> cc, string from, string title, string htmlBody, string textBody);
    }
}
