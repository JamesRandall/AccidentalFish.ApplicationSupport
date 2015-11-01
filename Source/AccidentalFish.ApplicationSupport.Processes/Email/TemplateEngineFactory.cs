using System;
using AccidentalFish.ApplicationSupport.Core.Templating;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    internal class TemplateEngineFactory : ITemplateEngineFactory
    {
        private readonly Lazy<ITemplateEngine> _handlebarsEngine = new Lazy<ITemplateEngine>(() => new HandlebarsTemplateEngine());

        public ITemplateEngine Create(TemplateSyntaxEnum syntax)
        {
            switch (syntax)
            {
                case TemplateSyntaxEnum.Handlebars:
                    return _handlebarsEngine.Value;
                case TemplateSyntaxEnum.Razor:
                    return new RazorTemplateEngine();
            }
            throw new NotSupportedException($"Template syntax of type {syntax} not supported");
        }
    }
}
