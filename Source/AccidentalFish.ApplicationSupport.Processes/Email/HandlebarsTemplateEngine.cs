using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using HandlebarsDotNet;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    internal class HandlebarsTemplateEngine : ITemplateEngine
    {
        private readonly ConcurrentDictionary<int, Func<object,string>> _compiledTemplates = new ConcurrentDictionary<int, Func<object, string>>(); 

        public string Execute(string template, Dictionary<string, string> mergeData)
        {
            int hashCode = template.GetHashCode();
            Func<object, string> compiledTemplate = _compiledTemplates.GetOrAdd(hashCode, key => Handlebars.Compile(template));
            string result = compiledTemplate(mergeData);
            return result;
        }
    }
}
