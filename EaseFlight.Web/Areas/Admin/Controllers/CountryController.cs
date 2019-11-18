using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.EntityModels;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class CountryController : BaseController
    {
        #region Properties
        private ICountryService CountryService { get; set; }
        #endregion

        #region Constructor
        public CountryController(ICountryService countryService)
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
                Name = collection.Get("country"),
                Region = collection.Get("region")
            };

            this.CountryService.Insert(country);
            TempData["msg"] = "success-Country add successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteCountry(int countryid)
        {
            this.CountryService.Delete(countryid);
            TempData["msg"] = "success-Country add successfully";

            return new JsonResult { ContentType = "text" };
        }

        [HttpPost]
        public ActionResult UpdateCountry(FormCollection collection)
        {
            var currentCountry = this.CountryService.Find(int.Parse(collection.Get("countryid")));

            currentCountry. Name = collection.Get("country");
            currentCountry.Region = collection.Get("region");

            this.CountryService.Update(currentCountry);
            TempData["msg"] = "success-Country update successfully";

            return RedirectToAction("Index");
        }
        #endregion
    }
}