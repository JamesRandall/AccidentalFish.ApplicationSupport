using System.Collections.Generic;
using RazorEngine;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    class RazorTemplateEngine : ITemplateEngine
    {
        public string Execute(string template, Dictionary<string, string> mergeData)
        {
            return Razor.Parse(template, mergeData);
        }
    }
}
