using System;
using System.Web.Mvc;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.Operations.Website.Filters
{
    public class LogPageRequestFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public LogPageRequestFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            _logger.Information(String.Format("Page request: {0}/{1}", filterContext.Controller.GetType().Name, filterContext.ActionDescriptor.ActionName));
        }

        /*public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            _logger.Information(String.Format("Page request: {0}/{1}", filterContext.Controller.GetType().Name, filterContext.ActionDescriptor.ActionName));
        }*/
    }
}