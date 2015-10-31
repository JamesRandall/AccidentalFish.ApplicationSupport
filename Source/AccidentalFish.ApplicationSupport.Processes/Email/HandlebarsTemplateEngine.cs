using System;
using System.Collections.Generic;
using HandlebarsDotNet;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    internal class HandlebarsTemplateEngine : ITemplateEngine
    {
        public string Execute(string template, Dictionary<string, string> mergeData)
        {
            Func<object,string> compiledTemplate = Handlebars.Compile(template);
            string result = compiledTemplate(mergeData);
            return result;
        }
    }
}
