using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Email
{
    public interface IEmailManager
    {
        Task Send(string to, string cc, string from, string emailTemplateId, Dictionary<string, string> mergeValues);

        Task Send(IEnumerable<string> to, IEnumerable<string> cc, string from, string emailTemplateId, Dictionary<string, string> mergeValues);

        Task Send(string to, string cc, string from, string subject, string body);

        Task Send(IEnumerable<string> to, IEnumerable<string> cc, string from, string subject, string body);
    }
}
