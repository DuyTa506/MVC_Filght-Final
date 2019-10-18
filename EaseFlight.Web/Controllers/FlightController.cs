using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    public class FlightController : Controller
    {
        [HttpPost]
        public ActionResult Find(FormCollection collection)
        {
            return View();
        }
    }
}