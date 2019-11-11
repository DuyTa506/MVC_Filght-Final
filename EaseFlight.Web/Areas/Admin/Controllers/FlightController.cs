using EaseFlight.BLL.Interfaces;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class FlightController : BaseController
    {
        #region Properties
        private IFlightService FlightService { get; set; }
        #endregion

        #region Constructor
        public FlightController(IFlightService flightService)
        {
            this.FlightService = flightService;            
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["flights"] = this.FlightService.FindAll();

            return View();
        }
        #endregion
    }
}