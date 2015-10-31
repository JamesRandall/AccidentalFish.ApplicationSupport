using System;
using AccidentalFish.ApplicationSupport.Core.Templating;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    internal class TemplateEngineFactory : ITemplateEngineFactory
    {
        public ITemplateEngine Create(TemplateSyntaxEnum syntax)
        {
            switch (syntax)
            {
                case TemplateSyntaxEnum.Handlebars:
                    return new HandlebarsTemplateEngine();
                case TemplateSyntaxEnum.Razor:
                    return new RazorTemplateEngine();
            }
            throw new NotSupportedException($"Template syntax of type {syntax} not supported");
        }
    }
}
