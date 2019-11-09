using EaseFlight.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EaseFlight.Web.Areas.Admin.Controllers
{
    public class PlaneAirportController : BaseController
    {

        #region Propertities
        private IPlaneAirportService PlaneAirportService { set; get; }

        #endregion

        #region Constructor
        public PlaneAirportController(IPlaneAirportService planeAirportService)
        {
            this.PlaneAirportService = planeAirportService;
        }
        #endregion

        #region Actions
        // GET: Admin/PlaneAirport
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["planes"] = this.PlaneAirportService.FindAll();

            return View();
        }

        #endregion  

    }
}