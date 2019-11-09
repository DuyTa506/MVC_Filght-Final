using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.EntityModels;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class AirportController : BaseController
    {
        #region Properties
        private IAirportService AirportService { get; set; }
        private ICountryService CountryService { get; set; }

        #endregion

        #region Constructor
        public AirportController(IAirportService airportService, ICountryService countryService)
        {
            this.AirportService = airportService;
            this.CountryService = countryService;
        }
        #endregion

        #region Action
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["airport"] = this.AirportService.FindAll();
            ViewData["countrys"] = this.CountryService.FindAll();
            return View();
        }
        [HttpPost]
        public ActionResult AddNewAirport(FormCollection collection)
        {
            var airport = new AirportModel
            {
                Name = collection.Get("name"),
                City = collection.Get("city"),
                CountryID = int.Parse(collection.Get("countryid"))
            };

            this.AirportService.Insert(airport);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteCountry(int airport)
        {
            this.AirportService.Delete(airport);

            return RedirectToAction("Index");
        }


        #endregion
    }
}