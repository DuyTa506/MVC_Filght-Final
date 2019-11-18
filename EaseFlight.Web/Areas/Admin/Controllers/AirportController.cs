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
            TempData["msg"] = "success-Country add successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateAirport(FormCollection collection)
        {
            var currentAirpot = this.AirportService.Find(int.Parse(collection.Get("airportid")));

            currentAirpot.Name = collection.Get("name");
            currentAirpot.City = collection.Get("city");
            currentAirpot.CountryID = int.Parse(collection.Get("countryid"));

            this.AirportService.Update(currentAirpot);
            TempData["msg"] = "success-Country update successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteAirport(int airportid)
        {
            this.AirportService.Delete(airportid);
            TempData["msg"] = "success-Country delete successfully";

            return new JsonResult { ContentType = "text" };
        }
        #endregion
    }
}