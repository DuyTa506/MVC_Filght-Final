using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class AirportController : Controller
    {
        // GET: Admin/Airport
        public ActionResult Index()
        {
            return View();
        }
    }
}