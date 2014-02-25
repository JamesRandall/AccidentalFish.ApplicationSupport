using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccidentalFish.Operations.Website.Domain.Services;
using AccidentalFish.Operations.Website.Domain.ViewModel;

namespace AccidentalFish.Operations.Website.Controllers
{
    public partial class AlertsController : Controller
    {
        private readonly IAlertSubscriberService _alertSubscriberService;

        public AlertsController(IAlertSubscriberService alertSubscriberService)
        {
            _alertSubscriberService = alertSubscriberService;
        }

        //
        // GET: /Alerts/
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Create()
        {
            return View(new AlertSubscriber());
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(AlertSubscriber model)
        {
            if (ModelState.IsValid)
            {
                await _alertSubscriberService.Create(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<JsonResult> Get(int page, int pageSize)
        {
            int totalRows;
            PageResult<AlertSubscriber> subscribers = await _alertSubscriberService.GetSubscribers(page, pageSize);
            return Json(new
            {
                currentPage = subscribers.Page,
                totalRows = subscribers.TotalRows
            }, JsonRequestBehavior.AllowGet);
        }
    }
}