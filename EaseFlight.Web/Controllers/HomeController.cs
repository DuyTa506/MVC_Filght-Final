using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.CustomModel;
using System.Collections.Generic;
using System.Linq;
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
            var airports = this.AirportService.FindAll();
            var airportRegion = new List<AirportRegionModel>();

            foreach(var airport in airports)
            {
                var airportList = airports.Where(a => a.Country.ID == airport.ID);
                airportRegion.Add(new AirportRegionModel
                {
                    Region = ""
                });
            }

            ViewData["airports"] = new {
                Region = "",
                Airports = this.AirportService.FindAll()
            };
            ViewData["seatClassList"] = this.SeatClassService.FindAll();

            return View();
        }
        #endregion
    }
}