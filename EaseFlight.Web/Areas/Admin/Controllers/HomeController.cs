using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.EntityModels;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        #region Properties
        private ICountryService CountryService { get; set; }

        #endregion

        #region Constructor
        public HomeController(ICountryService countryService)
        {
            this.CountryService = countryService;
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["countrys"] = this.CountryService.FindAll(); 
            
            return View();
        }
        [HttpPost]
        public ActionResult AddNewCountry(FormCollection collection)
        {
            var country = new CountryModel
            {
                Name = collection.Get("country"),Region =collection.Get("region")
            };

            this.CountryService.Insert(country);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteCountry(int countryId)
        {
            this.CountryService.Delete(countryId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCountry(FormCollection collection)
        {
            var country = new CountryModel
            {
                ID = int.Parse(collection.Get("countryid")),
                Name = collection.Get("country"),
                Region = collection.Get("region")
            };

            this.CountryService.Update(country);

            return RedirectToAction("Index");
        }
        #endregion

    }
}