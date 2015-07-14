using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Email
{
    public interface IEmailProvider
    {
        string Send(IEnumerable<string> to, IEnumerable<string> cc, string from, string title, string htmlBody, string textBody);
    }
}
