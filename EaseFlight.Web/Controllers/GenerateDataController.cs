using EaseFlight.BLL.Interfaces;
using EaseFlight.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Web.Controllers
{
    //Each Action will only run 1 time
    public class GenerateDataController : Controller
    {
        #region Properties
        private IAirportService AirportService { get; set; }
        private ISeatClassService SeatClassService { get; set; }
        private IPlaneService PlaneService { get; set; }
        private IPlaneSeatClassService PlaneSeatClassService { get; set; }
        private IPlaneAirportService PlaneAirportService { get; set; }
        #endregion

        #region Constructors
        public GenerateDataController(IAirportService airportService, ISeatClassService seatClassService,
            IPlaneService planeService, IPlaneSeatClassService planeSeatClassService, IPlaneAirportService planeAirportService)
        {
            this.AirportService = airportService;
            this.SeatClassService = seatClassService;
            this.PlaneService = planeService;
            this.PlaneAirportService = planeAirportService;
            this.PlaneSeatClassService = planeSeatClassService;
        }
        #endregion

        #region Actions
        public ActionResult GeneratePlaneAirport(int number)
        {
            var planeList = this.PlaneService.FindAll().ToList();
            var airportList = this.AirportService.FindAll().ToList();

            foreach(var plane in planeList)
            {
                var result = new List<AirportModel>();
                var random = new Random();

                for (var i = 0; i < number; ++i)
                {
                    var index = random.Next(airportList.Count());

                    if (result.IndexOf(airportList[index]) != -1)
                        --i;
                    else result.Add(airportList[index]);
                }

                for(var i = 0; i < number; ++i)
                {
                    var planeAirport = new PlaneAirportModel {
                        AirportID = result[i].ID,
                        PlaneID = plane.ID
                    };

                    this.PlaneAirportService.Insert(planeAirport);
                }
            }

            return View("Done");
        }

        [HttpGet]
        public ActionResult ViewDataGenerate()
        {
            var planeAirportList = this.PlaneAirportService.FindAll();

            ViewData["planeAirportList"] = planeAirportList;

            return View();
        }
        #endregion
    }
}