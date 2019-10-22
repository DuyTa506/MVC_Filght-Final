using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
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
        private ICountryService CountryService { get; set; }
        private IPassengerTypeService PassengerTypeService { get; set; }
        #endregion

        #region Constructors
        public HomeController(IAirportService airportService, ISeatClassService seatClassService,
            ICountryService countryService, IPassengerTypeService passengerTypeService)
        {
            this.AirportService = airportService;
            this.SeatClassService = seatClassService;
            this.CountryService = countryService;
            this.PassengerTypeService = passengerTypeService;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            var airports = this.AirportService.FindAll();
            var regions = this.CountryService.FindAll().Select(country => country.Region).Distinct();
            var passengers = this.PassengerTypeService.FindAll();
            var airportRegion = new List<AirportRegionModel>
            {
                new AirportRegionModel
                {
                    Region = Constant.CONST_DB_NAME_VIETNAM,
                    Airports = airports.Where(airport => airport.Country.Name.Equals(Constant.CONST_DB_NAME_VIETNAM))
                        .OrderBy(airport => airport.Name)
                }
            };

            foreach (var region in regions)
            {
                var airportList = airports.Where(airport => airport.Country.Region.Equals(region) 
                && !airport.Country.Name.Equals(Constant.CONST_DB_NAME_VIETNAM));

                if(airportList.Count() != 0)
                    airportRegion.Add(new AirportRegionModel
                    {
                        Region = region,
                        Airports = airportList
                    });
            }

            ViewData["airports"] = airportRegion;
            ViewData["passengers"] = passengers;
            ViewData["seatClassList"] = this.SeatClassService.FindAll();

            return View();
        }
        #endregion
    }
}