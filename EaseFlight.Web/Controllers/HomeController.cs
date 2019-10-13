using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Find()
        {
            return View();
        }
    }
}