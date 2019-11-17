using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.EntityModels;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {


            return View();
        }

    }
}