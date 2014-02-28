using System.Web;
using System.Web.Mvc;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.Operations.Website.Filters;

namespace AccidentalFish.Operations.Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            ILoggerFactory loggerFactory =
                (ILoggerFactory)
                    (DependencyResolver.Current.GetService(typeof(ILoggerFactory)));
            ILogger logger = loggerFactory.CreateLongLivedLogger(Constants.WebsiteComponentIdentity);

            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogPageRequestFilterAttribute(logger));
        }
    }
}
