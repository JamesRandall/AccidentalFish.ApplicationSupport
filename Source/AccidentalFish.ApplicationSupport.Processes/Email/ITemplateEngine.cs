using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    internal interface ITemplateEngine
    {
        string Execute(string template, Dictionary<string, string> mergeData);
    }
}
