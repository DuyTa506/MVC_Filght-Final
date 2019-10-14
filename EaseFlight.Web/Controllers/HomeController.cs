using EaseFlight.BLL.Interfaces;
using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Properties
        private IAirportService AirportService { get; set; }
        private ISeatClassService SeatClassService { get; set; }
        #endregion

        #region Constructors
        public HomeController(IAirportService airportService, ISeatClassService seatClassService)
        {
            this.AirportService = airportService;
            this.SeatClassService = seatClassService;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            ViewData["airports"] = this.AirportService.FindAll();
            ViewData["seatClassList"] = this.SeatClassService.FindAll();

            return View();
        }

        public ActionResult Find()
        {
            return View();
        }
        #endregion
    }
}