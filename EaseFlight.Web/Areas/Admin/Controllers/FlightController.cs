using EaseFlight.BLL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class FlightController : BaseController
    {
        #region Properties
        private IFlightService FlightService { get; set; }
        private IPlaneService PlaneService { get; set; }
        private IPlaneAirportService PlaneAirportService { get; set; }
        #endregion

        #region Constructor
        public FlightController(IFlightService flightService, IPlaneService planeService,
            IPlaneAirportService planeAirportService)
        {
            this.FlightService = flightService;
            this.PlaneService = planeService;
            this.PlaneAirportService = planeAirportService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["flights"] = this.FlightService.FindAll();
            ViewData["planes"] = this.PlaneService.FindAll().Where(plane => plane.PlaneAirports.Count > 1);

            return View();
        }

        [HttpPost]
        public JsonResult GetAirport(string planeId)
        {
            var result = new JsonResult { ContentType = "text" };
            var airports = this.PlaneAirportService.FindByPlane(int.Parse(planeId)).ToList();
            var data = new List<string>();

            airports.ForEach(air => data.Add(air.AirportID.ToString() + ":" + air.Airport.Name));
            result.Data = new { airport = data};

            return result;
        }
        #endregion
    }
}