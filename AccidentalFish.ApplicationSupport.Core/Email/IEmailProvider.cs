using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Email
{
    public interface IEmailProvider
    {
        Task<string> Send(IEnumerable<string> to, IEnumerable<string> cc, string from, string title, string htmlBody, string textBody);
    }
}
