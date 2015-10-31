using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Core.Templating;

namespace AccidentalFish.ApplicationSupport.Processes.Email
{
    internal interface ITemplateEngineFactory
    {
        ITemplateEngine Create(TemplateSyntaxEnum syntax);
    }
}
