using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccidentalFish.Operations.Website.Models
{
    public class ContinuationTokenViewModel
    {
        [AllowHtml]
        public string ContinuationToken { get; set; }
    }
}