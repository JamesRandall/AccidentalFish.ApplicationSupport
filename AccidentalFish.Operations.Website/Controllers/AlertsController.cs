using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccidentalFish.Operations.Website.Controllers
{
    public class AlertsController : Controller
    {
        //
        // GET: /Alerts/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
	}
}