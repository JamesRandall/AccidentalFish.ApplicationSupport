using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccidentalFish.Operations.Web.Startup))]
namespace AccidentalFish.Operations.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
