using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.Operations.Website.Domain.Services;
using AccidentalFish.Operations.Website.Models;

namespace AccidentalFish.Operations.Website.Controllers
{
    [Authorize]
    public partial class LogController : Controller
    {
        private readonly ILogViewerService _logViewerService;

        public LogController(ILogViewerService logViewerService)
        {
            _logViewerService = logViewerService;
        }

        
        public virtual ActionResult ViewByDateDescending()
        {
            return View();
        }

        public virtual ActionResult ViewBySeverity()
        {
            return View();
        }

        public virtual ActionResult ViewBySource()
        {
            return View();
        }

        [ValidateInput(false)]
        public virtual async Task<JsonResult> GetByDateDescending(string continuationToken = null)
        {
            PagedResultSegment<LogTableItem> results = await _logViewerService.GetByDateDescending(continuationToken);
            return Json(new {
                continuationToken = results.ContinuationToken,
                page = results.Page
                }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public virtual async Task<JsonResult> GetBySeverity(string continuationToken = null)
        {
            PagedResultSegment<LogTableItem> results = await _logViewerService.GetBySeverity(continuationToken);
            return Json(new
            {
                continuationToken = results.ContinuationToken,
                page = results.Page
            }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public virtual async Task<JsonResult> GetBySource(string continuationToken = null)
        {
            PagedResultSegment<LogTableItem> results = await _logViewerService.GetBySource(continuationToken);
            return Json(new
            {
                continuationToken = results.ContinuationToken,
                page = results.Page
            }, JsonRequestBehavior.AllowGet);
        }
	}
}